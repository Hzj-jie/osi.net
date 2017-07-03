
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

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

Public Class queue(Of T)
    Implements ICloneable

    Private ReadOnly data As list(Of T)

    Public Sub clear()
        data.clear()
    End Sub

    Public Function empty() As Boolean
        Return data.empty()
    End Function

    Public Function size() As UInt32
        Return data.size()
    End Function

    Public Function front() As T
        Return data.front()
    End Function

    Public Function back() As T
        Return data.back()
    End Function

    Public Function push(ByVal dataNew As T) As Boolean
        Return data.push_back(dataNew)
    End Function

    Public Function pop() As Boolean
        Return data.pop_front()
    End Function

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        Return New queue(Of T)(data)
    End Function

    'in fact, there should be no such function, but it's here, so leave them
    Public Function [erase](ByVal start As UInt32, ByVal [end] As UInt32) As Boolean
        While start < [end] AndAlso data.erase(start) <> data.end()
            start += uint32_1
        End While

        Return start = [end]
    End Function

    Public Function [erase](ByVal start As UInt32) As Boolean
        Return [erase](start, start + uint32_1)
    End Function

    Public Sub New(ByVal that As queue(Of T))
        copy(data, that.data)
    End Sub

    Public Sub New(ByVal that As list(Of T))
        copy(data, that)
    End Sub

    Public Sub New()
        data = New list(Of T)()
    End Sub
End Class
