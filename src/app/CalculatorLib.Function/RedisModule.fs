namespace CalculatorLib.Function

open System
open StackExchange.Redis
open System
open Models
open JsonModule
open SimpleLogger 

module RedisModule =
 
  //https://github.com/rgl/redis/downloads
  let redisConStr = 
    //ConfigurationManager.ConnectionStrings.["REDIS"].ConnectionString
    System.Environment.GetEnvironmentVariable "REDIS"
  
  let inline (~~) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit: ^a -> ^b) x)
  let getRedisResource() =
    "ERROR : Cannot obtain handle to redis cache "
    |> tryWith (fun x ->
                  log "initializaing redis cache ..."
                  let cx = ConnectionMultiplexer.Connect redisConStr
                  cx.GetDatabase()) 0

  let redisSet (k:string) obj =
    log "setting cache ..."
    log k
    match getRedisResource() with
    | Some cache -> 
        let result = serialize obj
        log result
        match result with
        | Some  v ->
          cache.StringSet(~~k, ~~v )
        | None -> log "ERROR : Problem serializing data for cache"
                  false
    | None -> log "ERROR : Unable to get redis resource"
              false 

  let redisGet (k:string) =
    log "getting cache ..."
    log k 
    match getRedisResource() with
    | Some cache -> cache.StringGet(~~k) |> (~~) |> sprintf "%s"
    | None -> log "ERROR : Unable to connect to redis"
              ""
    
