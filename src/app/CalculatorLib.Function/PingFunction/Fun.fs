namespace CalculatorLib.Function

open Microsoft.Azure.WebJobs.Host
open CalculatorlibCommon
open SimpleLogger
open System.Threading
open PromiseLib
open System.Net.Http

module PingFunction =

  let throttlingAgent = ThrottlingAgent("PingFunction")
  let trace (log:TraceWriter) x c = slog (sprintf "PingFunction - %A : %A " c x )|> log.Info
  let app (x:int) id = 
            Thread.Sleep x
            slog ("*** " + id) |> ignore
            x
  let run(req: HttpRequestMessage, log: TraceWriter) =
    let id = req |> getQueryParam "id"
    let result = throttlingAgent.RunWhenNotBusy (app 2000) id
                  |>Async.Start  
                 id
    result
