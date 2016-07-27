
Friend Class string_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_filter(i), s)
    End Sub
End Class

Friend Class string_care_case_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_care_case_filter(i), s)
    End Sub
End Class
