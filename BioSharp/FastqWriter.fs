namespace Sequences

open System
open System.IO
open Sequences

module FastqWriter =

    type FastqWriter ( outputStream:StreamWriter ) =
        //inherit SequenceWriter


        new ( fileName:string ) =
            let sw = new StreamWriter (fileName)
            FastqWriter(sw)            