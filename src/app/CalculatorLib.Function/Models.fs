namespace CalculatorLib.Function
open System
open System.Linq
open System
open System.Net

module Models =
  type RedisCacheIndexEntry = {
    PropertyName : string
    LastUpdated : DateTime
  } 
  type RedisCacheDocument = {
    RedisCacheIndexEntries : RedisCacheIndexEntry list
  }
   