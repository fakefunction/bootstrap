namespace CalculatorLibApp

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host

module CalculatorLibHttp =
  let run(req: HttpRequest, log: TraceWriter) =
    log.Info("F# HTTP trigger function processed a request.")
    log.Info(System.Configuration.ConfigurationManager.AppSettings.Count |> string)

    ContentResult(Content = "Boom! CalculatorLib .NET Core function works!", ContentType = "text/html")
