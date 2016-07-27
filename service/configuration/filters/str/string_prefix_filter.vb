
Imports osi.root.connector
Imports osi.root.template

Friend Class string_prefix_filter(Of CARE_CASE As _boolean)
    Inherits string_based_filter(Of CARE_CASE)

    Protected Sub New(ByVal base As String)
        MyBase.New(base)
    End Sub

    Protected NotOverridable Overrides Function match(ByVal input As String,
                                                      ByVal base As String,
                                                      ByVal care_case As Boolean) As Boolean
        Return strstartwith(input, base, care_case)
    End Function
End Class

Friend Class string_prefix_filter
    Inherits string_prefix_filter(Of _false)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_care_case_prefix_filter
    Inherits string_prefix_filter(Of _true)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_prefixes_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_prefix_filter(i), s)
    End Sub
End Class

Friend Class string_care_case_prefixes_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_care_case_prefix_filter(i), s)
    End Sub
End Class
