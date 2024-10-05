
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class error_context
    Public Shared Sub register(ByVal f As Func(Of String))
        global_resolver(Of Func(Of String), error_context).assert_first_register(f)
    End Sub

    Public Shared Function current() As String
        Dim f As Func(Of String) = Nothing
        If global_resolver(Of Func(Of String), error_context).resolve(f) Then
            Return f()
        End If
        Return Nothing
    End Function
End Class
