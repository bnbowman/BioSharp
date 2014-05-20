namespace Sequences

open System
open System.IO
open SequenceWriter
open Sequences

module FastaWriter =
    type FastaWriter private ( textWriter : TextWriter ) =
        //inherit SequenceWriter

        //Default constructor points the output stream to the console
        new () = FastaWriter( Console.Out )

        //Otherwise construction with a filename points the output to file
        new ( fileName:string ) =
            let streamWriter = new StreamWriter( fileName ) 
            FastaWriter( streamWriter )

        //override this.WriteRecord( record : SequenceRecord ) = ()

        //override this.WriteRecords( records : SequenceRecord[] ) = ()
            