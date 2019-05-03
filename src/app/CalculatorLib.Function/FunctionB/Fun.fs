namespace CalculatorLib.Function

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open Microsoft.Azure.WebJobs

module SomeFunctionB =

  let run(req: HttpRequest, log: TraceWriter) =
    log.Info("F# HTTP trigger function processed a request.")
    log.Info(System.Configuration.ConfigurationManager.AppSettings.Count |> string)
    ContentResult(Content = "FunctionB function works!!", ContentType = "text/html")