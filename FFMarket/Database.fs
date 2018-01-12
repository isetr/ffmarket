namespace FFMarket

    module Database =

        open FSharp.Data

        type DB = CsvProvider<"Content/sampledb.csv">

        let PrintDB (path: string) =
            use db = DB.Load path
            db.Rows
            |> Seq.iter (fun v -> printfn "%A" v)
