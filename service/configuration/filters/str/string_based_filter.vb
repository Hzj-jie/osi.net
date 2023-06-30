
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public MustInherit Class string_based_filter(Of case_sensitive As _boolean)
    Implements ifilter

    Private Shared ReadOnly cc As Boolean
    Private ReadOnly base As String

    Shared Sub New()
        cc = +(alloc(Of case_sensitive)())
    End Sub

    Protected Sub New(ByVal base As String)
        assert(Not base.null_or_empty())
        copy(Me.base, base)
    End Sub

    Protected MustOverride Function match(ByVal input As String,
                                          ByVal base As String,
                                          ByVal case_sensitive As Boolean) As Boolean

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        Return match(i, base, cc)
    End Function
End Class
