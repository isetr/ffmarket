namespace FFMarket

    module Database =

        open FSharp.Data

        type DB = CsvProvider<"Content/sampledb.csv">

        let Load (path: string) =
            DB.Load path

        let PrintDB (path: string) =
            use db = DB.Load path
            db.Rows
            |> Seq.iter (fun v -> printfn "%A" v)
