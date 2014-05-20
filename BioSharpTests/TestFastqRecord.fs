namespace SequenceTests

open System
open Sequences

module TestFastqRecord =
    let TestFastq =
        let reader1 = FastqReader.FastqReader( @"C:\Users\bbowman\Desktop\FSharpTest.fastq" )

        let rec1 = reader1.NextRecord()
        printfn "%A\n" rec1

        let rec2 = reader1.NextRecord()
        printfn "%A\n" rec2

        //let rec3 = reader1.NextRecord()
        //printfn "%A\n" rec3
        //reader1.Close()

        let reader2 = FastqReader.FastqReader( @"C:\Users\bbowman\Desktop\FSharpTest.fastq" )
        let records = reader2.Records() |> Seq.toList
        printfn "%i" records.Length
        for record in records do
            printfn "%A\n" record

        "Fastq Tests Passed"