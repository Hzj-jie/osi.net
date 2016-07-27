
Friend Class int_serial_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(x) New int_filter(x), s)
    End Sub
End Class
