
Imports System.IO

Public Module text_writer
    Public Function close_writer(ByVal i As TextWriter) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Try
                i.Flush()
                i.Close()
                i.Dispose()
                Return True
            Catch
                Return False
            End Try
        End If
    End Function
End Module
