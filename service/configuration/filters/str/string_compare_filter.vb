
Imports osi.root.template
Imports osi.root.utils

Friend Class string_compare_filter(Of CARE_CASE As _boolean)
    Inherits compare_filter(Of String)

    Public Sub New(ByVal s As String)
        MyBase.New(string_string_caster.instance, string_comparer(Of CARE_CASE).instance, s)
    End Sub
End Class

Friend Class string_compare_filter
    Inherits string_compare_filter(Of _false)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_care_case_compare_filter
    Inherits string_compare_filter(Of _true)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
