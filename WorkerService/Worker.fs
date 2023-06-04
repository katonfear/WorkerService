namespace WorkerService

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open System.Configuration
open System.Net.Sockets
open System.Net
open System.IO
open System.Text
open Newtonsoft.Json
open System.Collections.ObjectModel

exception Error1 of string

type Worker(logger: ILogger<Worker>) as self =
    inherit BackgroundService()
    
    override _.ExecuteAsync(ct: CancellationToken) =
        task {
             let out = self.CreateThread()
             out |> ignore
        }
    
    member this.Connection = new ConnectToDb("Shop", System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString)
    member this.Error1 = null
    member val Path: String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    member val Socket = null with get, set
    member val Shops: ObservableCollection<Shop> = new ObservableCollection<Shop>() with get

    member this.CreateThread() : int = 
        let out = this.ConnectClient() |> Async.StartAsTask
        0
    
    member this.ConnectClient() = async {
        try
            let port = System.Configuration.ConfigurationManager.AppSettings["port"]
            let ip = System.Configuration.ConfigurationManager.AppSettings["ip"]
            let localAddr = IPAddress.Parse(ip);
            this.Socket <- new TcpListener(localAddr, Convert.ToInt32(port))
            this.Socket.Start()
            let mutable outif = true
            while outif do
                let client = this.Socket.AcceptTcpClient()
                let out = this.ClientThread(client) |> Async.StartAsTask
                out |> ignore
        with | :? System.Exception as ex ->
                this.WriteLoger(ex.Message)
                this.WriteLoger(ex.StackTrace)
     }

     member this.GetDataFromDb(msg: string, data: string) : string =
        let mutable answer = String.Empty
        match msg with
            | "shops" -> 
                let tab = this.Connection.GetData("SELECT [id], [shopName] FROM [Shop].[dbo].[Shops]")
                JsonConvert.SerializeObject(tab)
            | "products" ->
                let tab = this.Connection.GetDataProc("[dbo].[GetProducts]")
                let prods = new List<Product>()
                for row in tab.Rows do
                    let prod = new Product(row["Name"].ToString(), row["BarCode"].ToString(), Convert.ToDouble(row["Price"]), row["ShopName"].ToString(), Convert.ToInt32(row["Qty"]))
                    prods.Add(prod)
                JsonConvert.SerializeObject(prods)
            | "addpord" ->
                try
                    let products = Product.FormJsonProducts(data)
                    printfn "dane2: %s"  data
                    for prod in products do
                        this.Connection.AddProduct(prod)
                    "{\"message\":\"all products was added\"}"
                with
                    | Error1(str) -> 
                        printfn @"%s" str
                        @"{""message"":" + "\"{" + str + "}\"}" 
            | _ -> String.Empty

    member this.SendAnswer(socket: TcpClient, msg: string) =
        let msgIn = msg.Split(char(2))
        let mutable msgval = ""
        let mutable data = ""
        if msgIn.Length > 1 then
            msgval <- msgIn[0].Trim()
            data <- msgIn[1].Trim()
        else
            msgval <- msgIn[0]
       // printfn "polecenie: %s, dane: %s" msgval data
        let answer = this.GetDataFromDb(msgval, data)
        use stream = socket.GetStream();
        if String.IsNullOrWhiteSpace(answer) then
            stream.Write(Encoding.UTF8.GetBytes("error"))
        else
            stream.Write(Encoding.UTF8.GetBytes(answer))

    member this.ClientThread(socket: TcpClient) = async {
        try
            use stream = socket.GetStream()
            let reader : BinaryReader = new BinaryReader(stream)
            let stringBuilder: StringBuilder = new StringBuilder()
            let buffer :byte array = Array.zeroCreate 1024
            use reader = new BinaryReader(stream)
            let start :int = 0
            let length :int  = 1024
            let mutable bytesRead = 1024
            while (bytesRead = 1024) do
                bytesRead <- reader.Read(buffer, start, length)
                let answer = Encoding.UTF8.GetString(buffer[0..bytesRead-1])
                let out = stringBuilder.Append(answer.Trim())
                out |> ignore
            this.SendAnswer(socket, stringBuilder.ToString())
        with    
            | :? System.Exception as ex ->
                this.WriteLoger(ex.Message)
                this.WriteLoger(ex.StackTrace)
    }

    member this.WriteLoger(msg: String) =
        try
            System.IO.File.AppendAllText(System.IO.Path.Combine(this.Path,"errors.txt"), $"{msg}{Environment.NewLine}") 
            |>ignore
        with
            | :? System.Exception as ex ->
                printf "%s" ex.Message
                |>ignore
    
            



