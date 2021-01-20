
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'process_io_wrapper_exe process_io_wrapper_pdb
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module _process_io_wrapper
    Public ReadOnly process_io_wrapper_exe() As Byte
    Public ReadOnly process_io_wrapper_pdb() As Byte

    Sub New()
        process_io_wrapper_exe = Convert.FromBase64String(strcat_hint(CUInt(4024), _
        "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"))

        assert(process_io_wrapper_exe.ungzip(process_io_wrapper_exe))
        process_io_wrapper_pdb = Convert.FromBase64String(strcat_hint(CUInt(2700), _
        "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"))

        assert(process_io_wrapper_pdb.ungzip(process_io_wrapper_pdb))
    End Sub
End Module
