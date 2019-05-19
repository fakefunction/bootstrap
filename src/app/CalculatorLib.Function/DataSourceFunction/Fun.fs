namespace CalculatorLib.Function


open Microsoft.Azure.WebJobs.Host
open CalculatorlibCommon
open SimpleLogger
open PromiseLib
open System.Net.Http

module DataSourceFunction =
  open System.Net.Http

  let throttleCacheUpdate = ThrottlingAgent("DataSourceFunction:throttleCachUpdate")
  let trace (log:TraceWriter) x c = slog (sprintf "DataSourceFunction - %A : %A " c x )|> log.Info
  let run(req: HttpRequestMessage, log: TraceWriter) =
    trace log "Processing incoming request ..." ""
    let id = req |> getQueryParam "id"
    trace log "Processing incoming request for propertyName" id
    id
