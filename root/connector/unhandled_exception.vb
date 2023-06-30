
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _unhandled_exception
    Private Sub export(ByVal ex As Exception, ByVal r As StringBuilder)
        assert(Not ex Is Nothing)
        assert(Not r Is Nothing)
        r.Append(ex.GetType().full_name()) _
         .Append(": source ") _
         .Append(ex.Source()) _
         .Append(", message ") _
         .Append(ex.Message()) _
         .Append(", stacktrace ") _
         .Append(newline.incode()) _
         .Append(ex.StackTrace())
        If Not ex.InnerException() Is Nothing Then
            r.Append(", inner exception: ") _
             .Append(newline.incode())
            export(ex.InnerException(), r)
        End If
    End Sub

    <Extension()> Public Function details(ByVal ex As Exception) As String
        If ex Is Nothing Then
            Return "null"
        Else
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            export(ex, r)
            Return Convert.ToString(r)
        End If
    End Function

    Public Sub log_unhandled_exception(ByVal prefix As String, ByVal ex As Exception)
        If Not ex Is Nothing Then
            raise_error(error_type.critical, prefix, ex.details())
        End If
    End Sub

    Public Sub log_unhandled_exception(ByVal ex As Exception)
        log_unhandled_exception("Caught unhandled exception: ", ex)
    End Sub
End Module
