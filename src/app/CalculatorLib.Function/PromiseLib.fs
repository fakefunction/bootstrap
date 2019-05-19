
namespace CalculatorLib.Function
open System
open SimpleLogger 
open System.Threading
module PromiseLib = 
    type ThrottlingAgent(context : string) = 
      static let monitor = Object()
      let mutable busy = false
      let ctx = context
      
      member this.RunWhenNotBusy f x = 
        if not busy then
          log (sprintf ">>QUEUED WORK : %A : ThrottlingAgent" ctx) 
          async {
              log (sprintf "%A : ThrottlingAgent : Task called" ctx) 
              Monitor.Enter monitor
              log (sprintf "%A : ThrottlingAgent : Task Working" ctx) 
              busy <- true
              try 
                f x |> ignore 
              with
              | e -> log (sprintf "Error %A : ThrottlingAgent : %A" ctx e)  
              log (sprintf "%A : ThrottlingAgent : Task Completed" ctx) 
              busy <- false 
              Monitor.Exit monitor
              return ()
            }            
        else
          log (sprintf ">>DISCARDED WORK : %A : ThrottlingAgent" ctx) 
          async { () } 
