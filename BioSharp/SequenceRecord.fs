namespace Sequences

open System
open System.IO
open System.Text.RegularExpressions

module SequenceRecord =
    // Module-level Exceptions for invalid records
    exception NameError of string
    exception SequenceError of string
    exception QualityError of string

    // Module-level functions for testing various arguments
    let private nameRegex = new Regex(@"[\x00-\x1F>]")
    let private seqRegex = new Regex(@"^[ACGT-]+$")
    let private qualRegex = new Regex(@"^[\x21-\x7E]+$")

    let IsValidName ( name:string ) = nameRegex.IsMatch(name) |> not
    let IsValidSequence ( sequence:string ) = seqRegex.IsMatch(sequence.ToUpper())
    let IsValidQuality ( quality:string ) = qualRegex.IsMatch(quality) || (quality.Length = 0) 

    // Internal utility functions
    let internal reverseComplementSequence ( sequence:seq<char> ) =
        let complement ( c:char ) =
            match c with
                | 'A' -> 'T'
                | 'C' -> 'G'
                | 'G' -> 'C'
                | 'T' -> 'A'
                | '-' -> '-'
                |  _  -> failwith "Invalid nucleotide supplied"
        let reverseComplement =
            sequence
            |> Seq.map complement
            |> Seq.toArray
            |> Array.rev
        new System.String (reverseComplement)

    let internal recordSerializer<'a> ( sw:StreamWriter) ( render:'a -> string ) ( records:'a seq ) =
        Seq.iter (fun r -> sw.WriteLine(render r)) records

    // Abstract parent class for both Fasta and Fastq Records
    [<AbstractClass>]
    type SequenceRecord( name:string, sequence:string, ?quality0:string ) =
        let quality = defaultArg quality0 ""

        do
            if not (IsValidName name) then 
                raise (NameError "Invalid characters in supplied name")
            if not (IsValidSequence sequence) then 
                raise (SequenceError("Invalid characters in supplied sequence"))
            if not (IsValidQuality quality) then 
                raise (QualityError("Invalid characters in supplied quality"))

        abstract Id : string with get
        abstract Name : string with get
        abstract Sequence : string  with get
        abstract Quality : string with get
        abstract ReverseComplement : SequenceRecord with get

        abstract member Slice : int * int -> SequenceRecord