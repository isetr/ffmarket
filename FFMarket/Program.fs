namespace FFMarket

    module Program =

        open System
        open FSharp.Charting
        open Model
            
        [<EntryPoint>]
        let main argv = 
            FromDB "Content/db.csv"
            |> SelectEntry "Fire Cluster" UnitsSold
            |> Map.toSeq
            |> Chart.Line
            |> Chart.Show
            0
