
Imports osi.root.template
Imports osi.root.connector

Public MustInherit Class string_based_filter(Of case_sensitive As _boolean)
    Implements ifilter

    Private Shared ReadOnly cc As Boolean
    Private ReadOnly base As String

    Shared Sub New()
        cc = +(alloc(Of case_sensitive)())
    End Sub

    Protected Sub New(ByVal base As String)
        assert(Not String.IsNullOrEmpty(base))
        copy(Me.base, base)
    End Sub

    Protected MustOverride Function match(ByVal input As String,
                                          ByVal base As String,
                                          ByVal case_sensitive As Boolean) As Boolean

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        Return match(i, base, cc)
    End Function
End Class
