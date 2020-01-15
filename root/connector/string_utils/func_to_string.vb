
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class func_to_string
    Private ReadOnly f As Func(Of String)

    Public Sub New(ByVal f As Func(Of String))
        assert(Not f Is Nothing)
        Me.f = f
    End Sub

    Public Overrides Function ToString() As String
        Return f()
    End Function
End Class
