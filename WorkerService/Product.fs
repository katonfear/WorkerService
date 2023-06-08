namespace WorkerService

open Newtonsoft.Json
open System.Collections.ObjectModel

type Product(nameIn: string, barcodeIn: string, priceIn: double, shopIn: string, qtyIn: double) =
    let mutable name: string = ""
    let mutable barcode: string = ""
    let mutable price: double = -1
    let mutable shop: string = ""
    let mutable qty: double = -1
    do
        name <- nameIn
        barcode <- barcodeIn
        price <- priceIn
        shop <- shopIn
        qty <- qtyIn

    member this.Name with get() = name and set(value) = name <-value
    member this.Barcode with get() = barcode and set(value) = barcode <-value
    member this.Shop with get() = shop and set(value) = shop <-value
    member this.Price with get() = price and set(value) = price <-value
    member this.Qty with get() = qty and set(value) = qty <-value

    member this.ToJson() =
        JsonConvert.SerializeObject(this)

    static member FormJson(json: string) : Product =
        JsonConvert.DeserializeObject<Product>(json)

    static member FormJsonProducts(json: string) : ObservableCollection<Product> =
        JsonConvert.DeserializeObject<ObservableCollection<Product>>(json)

    static member SerializeToJson(list: ObservableCollection<Product>) =
        JsonConvert.SerializeObject(list)
        
