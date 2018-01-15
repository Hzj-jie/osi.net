
Option Explicit On
Option Infer Off
Option Strict On

Friend Class int_compare_filter
    Inherits compare_filter(Of Int32)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
