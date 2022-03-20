
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public Module _queue
    <Extension()> Public Function null_or_empty(Of T)(ByVal this As queue(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function

    <Extension()> Public Function push(Of T) _
                                      (ByVal this As queue(Of T),
                                       ByVal v As vector(Of T)) As Boolean
        If this Is Nothing OrElse v.null_or_empty() Then
            Return False
        Else
            For i As UInt32 = uint32_0 To v.size() - uint32_1
                this.push(v(i))
            Next
            Return True
        End If
    End Function
End Module

Public NotInheritable Class queue(Of T)
    Implements ICloneable, ICloneable(Of queue(Of T))

    Private ReadOnly d As list(Of T)

    Private Sub New(ByVal that As list(Of T))
        assert(Not that Is Nothing)
        d = that.CloneT()
    End Sub

    <copy_constructor>
    Public Sub New(ByVal that As queue(Of T))
        Me.New(assert_which.of(that).is_not_null().d)
    End Sub

    Public Sub New()
        d = New list(Of T)()
    End Sub

    Public Sub clear()
        d.clear()
    End Sub

    Public Function empty() As Boolean
        Return d.empty()
    End Function

    Public Function size() As UInt32
        Return d.size()
    End Function

    Public Function front() As T
        Return d.front()
    End Function

    Public Function back() As T
        Return d.back()
    End Function

    Public Function push(ByVal v As T) As Boolean
        Return d.push_back(v)
    End Function

    Public Function emplace(ByVal v As T) As Boolean
        Return d.emplace_back(v)
    End Function

    Public Function pop() As Boolean
        Return d.pop_front()
    End Function

    Public Function pop(ByRef v As T) As Boolean
        If empty() Then
            Return False
        End If
        v = d.front()
        Return assert(d.pop_front())
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As queue(Of T) Implements ICloneable(Of queue(Of T)).Clone
        Return New queue(Of T)(d)
    End Function

    'in fact, there should be no such function, but it's here, so leave them
    Public Function [erase](ByVal start As UInt32, ByVal [end] As UInt32) As Boolean
        While start < [end] AndAlso d.erase(start) <> d.end()
            start += uint32_1
        End While

        Return start = [end]
    End Function

    Public Function [erase](ByVal start As UInt32) As Boolean
        Return [erase](start, start + uint32_1)
    End Function
End Class
