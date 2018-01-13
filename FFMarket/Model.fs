namespace FFMarket

    module Model =

        open Database

        type Entry =
            {
                Name        : string
                HQ          : bool
                Price       : int
                Quantity   : int
                Date        : System.DateTime
            }

        type Day = Map<System.DateTime, Map<string, Entry list>>

        let FromDB (path: string) =
            use db = Database.Load path
            db.Rows
            |> Seq.fold (fun (s: Day) v -> 
                let entry = {Name = v.Name; HQ = v.HQ; Price = v.Price; Quantity = v.Quantity; Date = v.Date}
                match s.TryFind v.Date with
                | Some m -> 
                    let el = 
                        match m.TryFind v.Name with
                        | Some el   -> entry :: el
                        | None      -> [entry]
                    let m = m.Add(v.Name, el)
                    s.Add (v.Date, m)
                | None -> 
                    let m = Map<string, Entry list> [|(v.Name, [entry])|]
                    s.Add (v.Date, m)
            ) Map.empty

        let Entries (db: Day) = 
            db
            |> Map.toSeq
            |> Seq.map (fun (_, v) ->
                v
                |> Map.toSeq
                |> Seq.map (fun (k, _) ->
                    k
                )
            )
            |> Seq.concat
            |> Seq.distinct
            |> Seq.toList

        let SelectEntry (entry: string) (selector: Entry list -> float) (db: Day) =
            db
            |> Map.map (fun k v ->
                match v.TryFind entry with
                | Some el -> selector el
                | None -> 0.
            )
            |> Map.filter (fun _ v ->
                v > 0.
            )

        let AveragePrice (entryList: Entry list) =
            entryList
            |> List.fold (fun s v -> s + v.Price * v.Quantity) 0
            |> fun x ->
                x / (entryList |> List.fold (fun s v -> s + v.Quantity) 0)
            |> float

        let UnitsSold (entryList: Entry list) =
            entryList
            |> List.fold (fun s v -> s + v.Quantity) 0
            |> float
