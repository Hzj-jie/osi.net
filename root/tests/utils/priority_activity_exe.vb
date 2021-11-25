
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'priority_activity_exe priority_activity_pdb
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module _priority_activity
    Public ReadOnly priority_activity_exe() As Byte
    Public ReadOnly priority_activity_pdb() As Byte

    Sub New()
        priority_activity_exe = Convert.FromBase64String(strcat_hint(CUInt(2884), _
        "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"))

        assert(priority_activity_exe.ungzip(priority_activity_exe))
        priority_activity_pdb = Convert.FromBase64String(strcat_hint(CUInt(2080), _
        "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"))

        assert(priority_activity_pdb.ungzip(priority_activity_pdb))
    End Sub
End Module
