
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Friend Class string_compare_filter(Of case_sensitive As _boolean)
    Inherits compare_filter(Of String)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub

    Protected NotOverridable Overrides Function compare(ByVal x As String, ByVal y As String) As Int32
        Return strcmp(Of case_sensitive)(x, y)
    End Function
End Class

Friend Class string_compare_filter
    Inherits string_compare_filter(Of _false)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_case_sensitive_compare_filter
    Inherits string_compare_filter(Of _true)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
