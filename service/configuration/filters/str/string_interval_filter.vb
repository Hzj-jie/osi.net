
Imports osi.root.template
Imports osi.root.utils

Friend Class string_interval_filter(Of case_sensitive As _boolean)
    Inherits interval_filter(Of String)

    Public Sub New(ByVal s As String)
        MyBase.New(string_string_caster.instance, string_comparer(Of case_sensitive).instance, s)
    End Sub
End Class

Friend Class string_interval_filter
    Inherits string_interval_filter(Of _false)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class

Friend Class string_case_sensitive_interval_filter
    Inherits string_interval_filter(Of _true)

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub
End Class
