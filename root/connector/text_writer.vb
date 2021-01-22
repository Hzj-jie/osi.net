
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO

Public Module text_writer
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    Public Function close_writer(ByVal i As TextWriter) As Boolean
        If i Is Nothing Then
            Return False
        End If
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
    End Function
End Module
