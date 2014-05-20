namespace SequenceTests

open System
open Sequences

module TestFastaRecord =
    let TestFasta =
        let name1 = "TestId1 TestDesc1"
        let seq1 = "ATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGCATGC"
        let record1 = FastaRecord.FastaRecord(name1, seq1)

        printfn "%s" record1.Id
        printfn "%s" record1.Name
        printfn "%s" record1.Sequence
        printfn "%i" record1.Sequence.Length
        printfn "%A\n" record1
        printfn "%A\n" record1.ReverseComplement

        let fastaString = ">TestId2 TestDesc2\nGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTAGCTA  \nGCTAGCTA  "
        let record2 = FastaRecord.FromString( fastaString )

        printfn "%A\n" record2
        printfn "%A\n" record2.ReverseComplement

        "Fasta Tests Passed"