namespace WorkerService

open Newtonsoft.Json
open System.Collections.ObjectModel

type Shop(id: int, name: string) as self =
    do  
        self.SetValue(id, name)

    member val Name: string = "" with get, set
    member val Id: int = id with get, set

    member this.SetValue(id: int, name: string) =
        this.Id <- id
        this.Name <- name
    
    member this.ToJson() =
        JsonConvert.SerializeObject(self)

    static member FormJson(json: string) : Shop =
        JsonConvert.DeserializeObject<Shop>(json)

    static member SerializeToJson(list: ObservableCollection<Shop>) =
        JsonConvert.SerializeObject(list)

    static member FormJsonShops(json: string) : ObservableCollection<Shop> =
        JsonConvert.DeserializeObject<ObservableCollection<Shop>>(json)

