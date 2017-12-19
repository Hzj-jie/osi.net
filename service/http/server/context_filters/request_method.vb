
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class request_methodAttribute
    Inherits context_filter

    Private ReadOnly m As constants.request_method

    Public Sub New(ByVal m As constants.request_method)
        Me.m = m
    End Sub

    Public Overrides Function [select](ByVal context As server.context) As Boolean
        If context Is Nothing Then
            Return False
        Else
            Return m = context.parse_method()
        End If
    End Function
End Class
