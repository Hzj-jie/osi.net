
Imports System.IO

Public Module text_writer
    Public Function close_writer(ByVal i As TextWriter) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim r As Boolean = False
            r = True
            Try
                i.Flush()
            Catch
                r = False
            End Try
            Try
                i.Close()
            Catch
                r = False
            End Try
            Try
                i.Dispose()
            Catch
                r = False
            End Try

            Return r
        End If
    End Function
End Module
