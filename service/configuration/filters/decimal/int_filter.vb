
Imports osi.root.utils

Friend Class int_filter
    Inherits cast_filter(Of Int32)

    Public Sub New(ByVal s As String)
        MyBase.New(string_int_caster.instance, int_comparer.instance, s)
    End Sub
End Class
