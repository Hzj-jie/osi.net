
Option Explicit On
Option Infer Off
Option Strict On

Friend Class double_interval_filter
    Inherits interval_filter(Of Double)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
