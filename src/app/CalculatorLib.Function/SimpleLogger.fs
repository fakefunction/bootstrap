namespace CalculatorLib.Function
open System
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

module SimpleLogger = 

(* 
  let slog x 
              ([<CallerMemberName; Optional; DefaultParameterValue("")>] memberName: string)
              ([<CallerFilePath; Optional; DefaultParameterValue("")>] path: string)
              ([<CallerLineNumber; Optional; DefaultParameterValue(0)>] line: int)  = 
    let logString  = sprintf "FUNCTION APP %A : %A" DateTime.Now x
    printfn "%A : %A Member : %A  Line : %A  File : %A" DateTime.Now x memberName line path
    logString
*)
  let slog x = 
    let logString  = sprintf "FUNCTION APP %A : %A" DateTime.Now x
    printfn "%A : %A " DateTime.Now x
    logString

  let log x = 
         slog x |> ignore

  let tryWith f x e  = 
    try 
    f x |> Some    
    with 
    | ex -> let toLog = (sprintf "%A %A" e ex)
            log toLog
            None