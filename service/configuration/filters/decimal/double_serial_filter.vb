
Option Explicit On
Option Infer Off
Option Strict On

Friend Class double_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(x) New double_filter(x), s)
    End Sub
End Class
