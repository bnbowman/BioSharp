namespace Sequences

open System
open System.IO

module SequenceWriter =

    [<AbstractClass>]
    type SequenceWriter private ( textWriter : TextWriter ) =
        
        //Default constructor points the output stream to the console
        new () = SequenceWriter( Console.Out )

        //Otherwise construction with a filename points the output to file
        new ( fileName:string ) =
            let streamWriter = new StreamWriter( fileName ) 
            SequenceWriter( streamWriter )

        abstract member WriteRecord : 'T -> unit
        abstract member WriteRecords : 'T[] -> unit