namespace CalculatorLib.Function

open FSharp.Data
open System
open RedisModule
open SimpleLogger
open MemCachingLib
open PromiseLib
open Models

module CalculatorlibCommon =    
    
    let MemoryCache = MemoryCache("properties")

    type CaheStrategy =
    | Redis
    
    open Microsoft.FSharp.Quotations
    open System.Net.Http

    //https://stackoverflow.com/questions/48304398/is-there-an-equivalent-of-cs-nameof-in-f
    (*
        The any definition is just a generic value that you can use to refer to instance members:
        nameof <@ any<System.Random>.Next @>
        nameof <@ System.Char.IsControl @>
    *)
    let nameof (q:Expr<_>) = 
        match q with 
        | Patterns.Let(_, _, DerivedPatterns.Lambdas(_, Patterns.Call(_, mi, _))) -> mi.Name
        | Patterns.PropertyGet(_, mi, _) -> mi.Name
        | DerivedPatterns.Lambdas(_, Patterns.Call(_, mi, _)) -> mi.Name
        | _ -> failwith "Unexpected format"

    let any<'R> : 'R = failwith "!"

    let getHostName() = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME")
    let getQueryParam  key (message:HttpRequestMessage) = 
        let value = List.ofSeq ( message.GetQueryNameValuePairs())
                    |> List.tryFind (fun item -> item.Key = key)
        match value with
        | Some x -> x.Value
        | None   -> ""
    
    let maxCacheAgeSeconds = 15.0
    
    let cacheExpired lastUpdated exp = lastUpdated < DateTime.UtcNow.AddSeconds(-exp)

    let cacheNotExpired lastUpdated exp = not (cacheExpired lastUpdated exp)
    
    let saveDataToCache propertyName data =
        let result = redisSet propertyName data
        data

    let GET<'a> dataServiceUrl =                
        log "Starting http request ..."
        log (sprintf "GET %A  ..." dataServiceUrl)                
        let response = Http.RequestString  (dataServiceUrl, httpMethod = "GET", headers = [ "Accept", "application/json" ])
        log "Serializing response ..."
        let cache = JsonModule.deserialize<'a> response
        log "Completed HTTP request"
        cache  

    let loadFromCacheWithoutInMemoryCache (propertyName : string) defaultData =
        log "Reaching out over the network to Redis"
        let result = redisGet propertyName
        let cache = JsonModule.deserialize result
        match cache with
        | Some data -> 
            {   Key = propertyName
                Value = data
                LastUpdated = data.LastUpdated }
        | None ->
            {   Key = propertyName
                Value = defaultData
                LastUpdated = DateTime() }         

    let loadFromCache 
      (strategy:CaheStrategy) 
      (propertyName : string) 
      caheExpirySeconds 
      defaultData 
      (throttler:ThrottlingAgent)  
      : string * DateTime * RedisCacheIndexEntry option =
        match strategy with
        | Redis ->
            log "Using redis only ..."
            let result = loadFromCacheWithoutInMemoryCache propertyName defaultData
            result.Key, result.LastUpdated, Some result.Value            