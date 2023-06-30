
Option Explicit On
Option Infer Off
Option Strict On

Public Class cast_filter(Of T)
    Inherits cast_compare_validation(Of T, T)
    Implements ifilter

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub

    Public Function match(ByVal s As String) As Boolean Implements ifilter.match
        Dim j As T = Nothing
        Return validate() AndAlso cast(s, j) AndAlso equal(store(), j)
    End Function

    Protected NotOverridable Overrides Function parse(ByVal s As String, ByRef o As T) As Boolean
        Return cast(s, o)
    End Function
End Class
