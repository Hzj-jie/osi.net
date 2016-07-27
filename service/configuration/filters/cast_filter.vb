
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public Class cast_filter(Of T)
    Inherits cast_compare_validation(Of T, T)
    Implements ifilter

    Public Sub New(ByVal caster As icaster(Of String, T), ByVal comparer As icomparer(Of T), ByVal s As String)
        MyBase.New(caster, comparer, s)
    End Sub

    Public Function match(ByVal s As String) As Boolean Implements ifilter.match
        Dim j As T
        Return validate() AndAlso cast(s, j) AndAlso equal(store(), j)
    End Function

    Protected NotOverridable Overrides Function parse(ByVal s As String, ByRef o As T) As Boolean
        Return cast(s, o)
    End Function
End Class
