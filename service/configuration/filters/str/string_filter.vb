
Imports osi.root.connector
Imports osi.root.template

Friend Class string_filter(Of case_sensitive As _boolean)
    Inherits string_based_filter(Of case_sensitive)

    Protected Sub New(ByVal base As String)
        MyBase.New(base)
    End Sub

    Protected NotOverridable Overrides Function match(ByVal input As String,
                                                      ByVal base As String,
                                                      ByVal case_sensitive As Boolean) As Boolean
        Return strsame(input, base, case_sensitive)
    End Function
End Class

Friend Class string_filter
    Inherits string_filter(Of _false)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_case_sensitive_filter
    Inherits string_filter(Of _true)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
