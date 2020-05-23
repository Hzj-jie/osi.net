
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module _set
    Private Function insert(Of T)(ByVal this As bbst(Of T),
                                  ByVal v As vector(Of T),
                                  ByVal emplace As Boolean) As Boolean
        If this Is Nothing OrElse v Is Nothing Then
            Return False
        ElseIf v.empty() Then
            Return True
        Else
            If emplace Then
                For i As UInt32 = 0 To v.size() - uint32_1
                    this.emplace(v(i))
                Next
            Else
                For i As UInt32 = 0 To v.size() - uint32_1
                    this.insert(v(i))
                Next
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function insert(Of T)(ByVal this As bbst(Of T), ByVal v As vector(Of T)) As Boolean
        Return insert(this, v, False)
    End Function

    <Extension()> Public Function emplace(Of T)(ByVal this As bbst(Of T), ByVal v As vector(Of T)) As Boolean
        Return insert(this, v, True)
    End Function

    <Extension()> Public Function null_or_empty(Of T)(ByVal this As [set](Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function

    <Extension()> Public Function has(Of T)(ByVal this As [set](Of T), ByVal v As T) As Boolean
        Return Not this Is Nothing AndAlso this.find(v) <> this.end()
    End Function
End Module

Public NotInheritable Class [set]
    Private Shared Function create(Of T)(ByVal vs() As T, ByVal require_copy As Boolean) As [set](Of T)
        Dim r As [set](Of T) = Nothing
        r = New [set](Of T)()
        For i As Int32 = 0 To array_size_i(vs) - 1
            If require_copy Then
                r.insert(vs(i))
            Else
                r.emplace(vs(i))
            End If
        Next
        Return r
    End Function

    Public Shared Function [of](Of T)(ByVal ParamArray vs() As T) As [set](Of T)
        Return create(vs, True)
    End Function

    Public Shared Function emplace_of(Of T)(ByVal ParamArray vs() As T) As [set](Of T)
        Return create(vs, False)
    End Function

    Private Sub New()
    End Sub
End Class
