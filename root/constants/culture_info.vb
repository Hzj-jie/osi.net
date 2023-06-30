
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Globalization

Public NotInheritable Class culture_info
    Public Shared ReadOnly en_US As CultureInfo = CultureInfo.GetCultureInfo("en-US")
    Public Shared ReadOnly zh_CN As CultureInfo = CultureInfo.GetCultureInfo("zh-CN")

    Private Sub New()
    End Sub
End Class
