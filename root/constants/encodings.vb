
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text

Public NotInheritable Class encodings
    Public Shared ReadOnly gbk As Encoding
    Public Shared ReadOnly gbk_or_default As Encoding

    Shared Sub New()
        Try
            gbk = Encoding.GetEncoding("gbk")
        Catch
        End Try
        If gbk Is Nothing Then
            gbk_or_default = Encoding.Default
        Else
            gbk_or_default = gbk
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
