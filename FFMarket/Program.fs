namespace FFMarket

    module Program =

        open System
        open FSharp.Charting
        open Model
            
        [<EntryPoint>]
        let main argv = 
            printf "Enter the name of the item: "
            let entry = Console.ReadLine()
            let fromdb = FromDB "Content/db.csv"
            let selectors = 
                [
                    UnitsSold, "Units sold"
                    AveragePrice, "Average Price"
                ]
            selectors
            |> List.map (fun selector ->
                fromdb
                |> SelectEntry entry (fst selector)
                |> Map.toSeq
                |> (fun x -> 
                    let labels = Seq.map (fun e -> string <| snd e) x
                    let name = (snd selector) + " " + entry
                    Chart.Column (x, Labels=labels, Name=name)
                )
            )
            |> Chart.Combine
            |> Chart.Show
            0
