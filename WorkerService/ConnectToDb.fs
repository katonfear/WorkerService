namespace WorkerService

open Microsoft.Data.SqlClient
open System.Data

type ConnectToDb(name: string, connetrinoString: string) =

    member this.Name = name

    member this.ConnectionString = connetrinoString

    member this.GetData(query: string) : DataTable =
        let tab = new DataTable()
        use sqlConn = new SqlConnection(connetrinoString)
        sqlConn.Open();
        use cmd = sqlConn.CreateCommand()
        cmd.CommandText <- query
        use da = new SqlDataAdapter(cmd)
        let ot = da.Fill(tab)
        tab

    member this.GetDataProc(query: string) : DataTable =
        use sqlConn = new SqlConnection(connetrinoString)
        sqlConn.Open();
        let tab = new DataTable()
        use cmd = sqlConn.CreateCommand()
        cmd.CommandText <- query
        cmd.CommandType <- CommandType.StoredProcedure
        use da = new SqlDataAdapter(cmd)
        let ot = da.Fill(tab)
        tab

    member this.AddProduct(product: Product) =
        let query = "[dbo].[AddProduct]";
        use sqlConn = new SqlConnection(connetrinoString)
        sqlConn.Open();
        use cmd = sqlConn.CreateCommand()
        cmd.CommandType <- CommandType.StoredProcedure
        cmd.CommandText <- query
        let mutable param = cmd.Parameters.AddWithValue("Name", product.Name);
        param <- cmd.Parameters.AddWithValue("Barcode", product.Barcode);
        param <- cmd.Parameters.AddWithValue("Shop", product.Shop);
        param <- cmd.Parameters.AddWithValue("Price", product.Price);
        param <- cmd.Parameters.AddWithValue("Qty", product.Qty);
        let out = cmd.ExecuteNonQuery()
        0|> ignore
