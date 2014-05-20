namespace Sequences

open System
open System.IO
open SequenceRecord

module FastqRecord =
    exception FastqFormatError of string
    exception IncompleteRecordError of string
    
    let private fstFastqDelim = '@'
    let private sndFastqDelim = '+'

    let private fastqRender ( sr:SequenceRecord ) =
        sprintf "@%s\n%s\n+\n%s\n" sr.Name sr.Sequence sr.Quality


    let FastqWriter records =
        use sw = new StreamWriter (Console.OpenStandardOutput())
        recordSerializer sw fastqRender records


    type FastqRecord( name:string, sequence:string, quality:string ) =
        inherit SequenceRecord(name, sequence, quality)

        static let isValidLength ( sequence:string, quality:string ) = (sequence.Length = quality.Length)

        do
            if not (isValidLength (sequence, quality)) then 
                raise (FastqFormatError "Lengths of sequence and quality don't match")

        let nameParts = name.Split(' ')
        
        override this.Id = nameParts.[0]
        override this.Name = name
        override this.Sequence = sequence.ToUpper()
        override this.Quality = quality

        override this.ReverseComplement =
            let revComSeq = reverseComplementSequence this.Sequence
            let revComQual = System.String(this.Quality.ToCharArray() |> Array.rev)
            FastqRecord( this.Name, revComSeq, revComQual ) :> SequenceRecord

        override this.Slice( start:int, finish:int ) =
            let slicedSequence = this.Sequence.Substring(start, finish)
            let slicedQuality = this.Quality.Substring(start, finish)
            FastqRecord( this.Name, slicedSequence, slicedQuality ) :> SequenceRecord

        override this.ToString() =
            fastqRender this
    // End FastqRecord declaration


    let FromString( fastqString:string ) =
        let lines = fastqString.Split('\n')
        let numLines = lines.Length

        if numLines <> 4 then
            let msg = sprintf "%i lines found, but 4 required" numLines
            raise (IncompleteRecordError msg)

        let fstDelim = lines.[0].[0]
        let fstName = lines.[0].Substring(1).Trim()
        let sequence = lines.[1].Trim()
        let sndDelim = lines.[2].[0]
        let sndName = lines.[2].Substring(1).Trim()
        let quality = lines.[3].Trim()

        if fstDelim <> fstFastqDelim then 
            let msg = sprintf "First record delimiter was '%c' instead of '%c'" fstDelim fstFastqDelim
            raise (FastqFormatError msg)
        if sndDelim <> sndFastqDelim then 
            let msg = sprintf "Second record delimiter was '%c' instead of '%c'" sndDelim sndFastqDelim
            raise (FastqFormatError msg)

        FastqRecord( fstName, sequence, quality )
    // End FromString Declaration