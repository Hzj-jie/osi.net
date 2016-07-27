
Imports osi.root.utils

Friend Class double_compare_filter
    Inherits compare_filter(Of Double)

    Public Sub New(ByVal s As String)
        MyBase.New(string_double_caster.instance, double_comparer.instance, s)
    End Sub
End Class
