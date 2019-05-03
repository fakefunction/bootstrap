namespace CalculatorLib.Function

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open Microsoft.Azure.WebJobs

module SomeFunctionA =

  let run(req: HttpRequest, log: TraceWriter) =
    log.Info("F# HTTP trigger function processed a request.")
    log.Info(System.Configuration.ConfigurationManager.AppSettings.Count |> string)
    ContentResult(Content = "FunctionA function works!!", ContentType = "text/html")
