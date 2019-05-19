namespace CalculatorLib.Function

open System
open System.Collections.Generic
open System.Threading

module MemCachingLib =
    type InMemoryCacheItem<'a> = {
        Key : string
        LastUpdated : DateTime
        Value : 'a
    }
    type InMemoryCache<'a> = 
        {
            Data : Dictionary<string, InMemoryCacheItem<'a>>
        }

    type MemoryCache(scope: string) = 
        
        static let mutable _cache : InMemoryCache<'a> = { 
                Data = new Dictionary<string, InMemoryCacheItem<'a>>()
            }
        static let monitor = Object()

        member this.Scope = scope

        member this.Get(key) = 
            if _cache.Data.ContainsKey(this.Scope+":"+key) 
            then Some _cache.Data.[this.Scope+":"+key]
            else None
        
        member this.Set(k,v) = 
            Monitor.Enter monitor
            match this.Get(k) with
            | Some va -> _cache.Data.[this.Scope+":"+k] <- v
            | None   -> _cache.Data.Add(this.Scope+":"+k,v)
            Monitor.Exit monitor

        member this.Remove(k) = 
            Monitor.Enter monitor
            match this.Get(k) with
            | Some va -> _cache.Data.Remove (this.Scope+":"+k) |> ignore
            | None   -> false  |> ignore
            Monitor.Exit monitor
