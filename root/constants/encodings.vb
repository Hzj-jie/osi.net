
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text

Public NotInheritable Class encodings
    Public Shared ReadOnly gbk As Encoding
    Public Shared ReadOnly gbk_or_default As Encoding
    Public Shared ReadOnly utf8_nobom As Encoding

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
        utf8_nobom = New UTF8Encoding(False, False)
    End Sub

    Private Sub New()
    End Sub
End Class
