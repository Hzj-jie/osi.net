
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text

Public NotInheritable Class encodings
    Public Shared ReadOnly gbk As Encoding = executor.instance.gbk
    Public Shared ReadOnly gbk_or_default As Encoding = executor.instance.gbk_or_default
    Public Shared ReadOnly utf8_nobom As Encoding = executor.instance.utf8_nobom

    Private NotInheritable Class executor
        Public Shared ReadOnly instance As executor = New executor()

        Public ReadOnly gbk As Encoding
        Public ReadOnly gbk_or_default As Encoding
        Public ReadOnly utf8_nobom As Encoding

        Private Sub New()
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
    End Class

    Private Sub New()
    End Sub
End Class
