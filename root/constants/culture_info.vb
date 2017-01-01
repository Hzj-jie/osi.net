
Imports System.Globalization

Public NotInheritable Class culture_info
    Public Shared ReadOnly en_US As CultureInfo
    Public Shared ReadOnly zh_CN As CultureInfo

    Shared Sub New()
        en_US = CultureInfo.GetCultureInfo("en-US")
        zh_CN = CultureInfo.GetCultureInfo("zh-CN")
    End Sub

    Private Sub New()
    End Sub
End Class
