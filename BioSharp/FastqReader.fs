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

        let lineQuartet readLines = 
            let quartet = Seq.take 4 readLines
            match quartet with
                | IsEmpty -> null
                | _ -> quartet 

        let linesToString lineSeq = String.concat "\n" lineSeq            

        let stringToRecord recordString = FastqRecord.FromString recordString

        let nextRecord readLines = 
            lineQuartet readLines
            |> linesToString
            |> stringToRecord

        let records = 
            readLines 
            |> Seq.unfold (fun readLines -> Some(nextRecord readLines, readLines)) 

        member this.Records() = records

        member this.NextRecord() = Seq.head records

        member this.Close() = inputStream.Close()

        new ( fileName:string ) =
            let reader = File.OpenText(fileName)
            FastqReader(reader)