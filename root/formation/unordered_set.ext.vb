
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _unordered_set
    <Extension()> Public Function stream(Of T)(ByVal this As unordered_set(Of T)) As stream(Of T)
        Return New stream(Of T).container(Of unordered_set(Of T))(this)
    End Function

    <Extension()> Public Function null_or_empty(Of T)(ByVal this As unordered_set(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module

Public NotInheritable Class unordered_set
    Public Shared Function [of](Of T)(ByVal ParamArray vs() As T) As unordered_set(Of T)
        Dim r As New unordered_set(Of T)()
        For Each v As T In vs
            r.insert(v)
        Next
        Return r
    End Function

    Public Shared Function emplace_of(Of T)(ByVal ParamArray vs() As T) As unordered_set(Of T)
        Dim r As New unordered_set(Of T)()
        For Each v As T In vs
            r.emplace(v)
        Next
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
