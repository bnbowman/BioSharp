namespace Sequences

open System 
open System.IO
open Sequences

module FastqReader =
    
    let (|IsEmpty|_|) ( s:seq<'T> ) =
        if Seq.isEmpty s then Some()
        else None

    type FastqReader( inputStream:StreamReader ) = 

        let readLines = seq {
            while not inputStream.EndOfStream do
                yield inputStream.ReadLine()
        }

        let lineQuartets = seq {
            yield Seq.take 4 readLines
        }

        //let lineQuartet readLines = 
        //    let quartet = Seq.take 4 readLines
        //    printfn "A %A" quartet
        //    match quartet with
        //        | IsEmpty -> None
        //        | _ -> Some quartet 

        let linesToString lineSeq = String.concat "\n" lineSeq            

        let stringToRecord recordString = FastqRecord.FromString recordString

        let linesToRecord lineSeq = lineSeq |> linesToString |> stringToRecord 

        let nextRecord readLines =
            let quartet = Seq.head lineQuartets
            //let quartet = lineQuartet readLines
            printfn "B %A" quartet
            quartet
            //match quartet with
            //    | None -> None
            //    | Some(quartet) -> Some (linesToRecord quartet)

        let nextRecord2 = seq {
            while not (Seq.isEmpty lineQuartets) do
                let x = Seq.head lineQuartets |> linesToRecord
                yield x
        }

        let records2 = nextRecord2

        let records = 
            readLines 
            |> Seq.unfold (fun readLines -> Some(nextRecord readLines, readLines)) 

        member this.Records() = records2

        member this.NextRecord() = Seq.head records2

        new ( fileName:string ) =
            let reader = File.OpenText(fileName)
            FastqReader(reader)