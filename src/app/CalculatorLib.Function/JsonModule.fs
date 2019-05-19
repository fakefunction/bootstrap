namespace CalculatorLib.Function
open Newtonsoft.Json
open SimpleLogger 
open System

module JsonModule =
    
  let serialize obj =
    log "serializing object..." 
    "Error serializing object"
    |> tryWith JsonConvert.SerializeObject obj 

  let deserialize<'a> str =
    log "deserialize ..."
    if str |> String.IsNullOrEmpty then 
        log "Error deserializing object : empty string provided"
        None
    else    
        "Error deserializing object"
        |> tryWith JsonConvert.DeserializeObject<'a> str  