namespace Sequences

open System
open System.IO
open SequenceRecord

module FastaRecord =
    exception FastaFormatError of string

    let private fastaDelim = '>'

    // Declare a module-level function for wrapping sequence text
    let private wrapSize = 60
    let rec private wrappedSequence ( sequence:string ) =
        if sequence.Length <= wrapSize then
            sequence
        else
            let prefix = sequence.Substring(0, wrapSize)
            let suffix = sequence.Substring(wrapSize)
            prefix + "\n" + wrappedSequence suffix

    let private fastaRender ( sr:SequenceRecord ) =
        sprintf ">%s\n%s\n" sr.Name (wrappedSequence sr.Sequence)


    let FastaWriter records =
        use sw = new StreamWriter (Console.OpenStandardOutput())
        recordSerializer sw fastaRender records


    type FastaRecord( name:string, sequence:string ) =
        inherit SequenceRecord(name, sequence)

        let nameParts = name.Split(' ')
        
        override this.Id = nameParts.[0]
        override this.Name = name
        override this.Sequence = sequence.ToUpper()
        override this.Quality = ""

        override this.ReverseComplement =
            let revComSeq = reverseComplementSequence this.Sequence
            FastaRecord( this.Name, revComSeq ) :> SequenceRecord

        override this.Slice( start:int, finish:int ) =
            FastaRecord( this.Name, this.Sequence.Substring(start, finish) ) :> SequenceRecord

        override this.ToString() =
            fastaRender this
    // End FastaRecord Declaration


    let FromString( fastaString:string ) =
        let lines = fastaString.Split('\n')
        let delim = lines.[0].[0]
        let name = lines.[0].Substring(1)
        let sequence = 
            lines.[1..] |>
            Array.map (fun s -> s.Trim()) |>
            String.concat ""

        if delim <> fastaDelim then 
            let msg = sprintf "Record delimiter was '%c' instead of '%c'" delim fastaDelim
            raise (FastaFormatError msg)

        FastaRecord( name, sequence )
    // End FromString Declaration