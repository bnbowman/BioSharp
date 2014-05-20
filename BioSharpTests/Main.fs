namespace SequenceTests

open TestFastaRecord
open TestFastqRecord

module Main =
    [<EntryPoint>]
    let main argv =
        printfn "%s" TestFasta
        printfn "%s" TestFastq
        
        0