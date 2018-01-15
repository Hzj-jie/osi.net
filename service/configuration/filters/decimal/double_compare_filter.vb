
Option Explicit On
Option Infer Off
Option Strict On

Friend Class double_compare_filter
    Inherits compare_filter(Of Double)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
