
Option Explicit On
Option Infer Off
Option Strict On

Friend Class string_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_filter(i), s)
    End Sub
End Class

Friend Class string_case_sensitive_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_case_sensitive_filter(i), s)
    End Sub
End Class
