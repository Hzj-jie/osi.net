
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class tar
    Public Shared Function memory_stream_from_string(ByVal s As String, ByVal o As MemoryStream) As Boolean
        memory_stream.of(s).CopyTo(o)
        Return True
    End Function

    Public Shared Function dump_to_memory_streams(ByVal v As vector(Of MemoryStream)) _
                                                 As Func(Of UInt32, MemoryStream, Boolean)
        assert(Not v Is Nothing)
        Return Function(ByVal index As UInt32, ByVal i As MemoryStream) As Boolean
                   assert(v.size() = index)
                   v.emplace_back(i.clone())
                   Return True
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
