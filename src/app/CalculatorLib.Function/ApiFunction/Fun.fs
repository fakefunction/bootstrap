namespace CalculatorLib.Function

open Microsoft.Azure.WebJobs.Host
open CalculatorlibCommon
open SimpleLogger
open PromiseLib
open DataSourceFunction
open System.Net.Http

module ApiFunction =    

    let throttler = ThrottlingAgent("ApiFunction")
    let trace (log : TraceWriter) x c = slog (sprintf "ApiFunction - %A : %A " c x) |> log.Info
    
    let run (req : HttpRequestMessage, log : TraceWriter) =
        async {                        
            let id = req |> getQueryParam "id"
            let dataServiceUrl = "http://" + getHostName() + "/api/DataSourceFunction?id=" + id
            throttler.RunWhenNotBusy GET dataServiceUrl
            |>Async.Start         
            return match GET dataServiceUrl with
                    | Some data -> data
                    | None       -> ""
        } |> Async.StartAsTask