namespace FFMarket

    module Program =

        open System
        open FSharp.Charting
        open Model
            
        [<EntryPoint>]
        let main argv = 
            //printf "Enter the name of the item: "
            //let entry = Console.ReadLine()
            let fromdb = FromDB "Content/db.csv"
            let entries = Entries fromdb
            let selectors = 
                [
                    UnitsSold, "Units sold"
                    AveragePrice, "Average Price"
                ]
            entries
            |> List.map (fun entry ->
                selectors
                |> List.map (fun selector ->
                    fromdb
                    |> SelectEntry entry (fst selector)
                    |> Map.toSeq
                    |> (fun x -> 
                        let labels = Seq.map (fun e -> string <| snd e) x
                        let name = sprintf "%s (%s)" (snd selector) entry
                        Chart.Column (x, Labels=labels, Name=name, Title=entry)
                    )
                )
                |> Chart.Combine
            )
            |> Seq.iter Chart.Show
            0
