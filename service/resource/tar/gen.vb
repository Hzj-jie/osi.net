
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates

Partial Public NotInheritable Class tar
    Public NotInheritable Class gen
        Private Shared pattern As argument(Of String)
        Private Shared size As argument(Of UInt32)

        Public Shared Function read() As MemoryStream
            Dim o As New MemoryStream()
            assert(writer.in_mem(size Or 10 * 1024 * 1024,
                                 New selector() With {.pattern = pattern Or "*"},
                                 o).dump())
            Return o
        End Function

        Public Shared Function reader_of(ByVal input As MemoryStream) As reader
            Return reader.in_mem(input)
        End Function

        Public Shared Function reader_of(ByVal input() As Byte) As reader
            Return reader_of(memory_stream.of(input))
        End Function

        Public Shared Sub dump(ByVal input As MemoryStream, ByVal root As String)
            reader_of(input).foreach(Sub(ByVal file As String, ByVal m As MemoryStream)
                                         assert(Not file Is Nothing)
                                         assert(Not m Is Nothing)
                                         Try
                                             Directory.GetParent(Path.Combine(root, file)).Create()
                                         Catch ex As Exception
                                             assert(False, ex)
                                         End Try
                                         IO.File.WriteAllBytes(Path.Combine(root, file), m.bytes())
                                     End Sub)
        End Sub

        Public Shared Sub dump(ByVal input As MemoryStream)
            dump(input, Environment.CurrentDirectory())
        End Sub

        Public Shared Sub dump(ByVal input() As Byte, ByVal root As String)
            dump(memory_stream.of(input), root)
        End Sub

        Public Shared Sub dump(ByVal input() As Byte)
            dump(input, Environment.CurrentDirectory())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
