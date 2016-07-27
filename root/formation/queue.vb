
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

Public Class queue(Of valueT)
    Implements ICloneable

    Private ReadOnly data As list(Of valueT)

    Public Sub clear()
        data.clear()
    End Sub

    Public Function empty() As Boolean
        Return data.empty()
    End Function

    Public Function size() As Int64
        Return data.size()
    End Function

    Public Function front() As valueT
        Return data.front()
    End Function

    Public Function back() As valueT
        Return data.back()
    End Function

    Public Function push(ByVal dataNew As valueT) As Boolean
        Return data.push_back(dataNew)
    End Function

    Public Function pop() As Boolean
        Return data.pop_front()
    End Function

    Public Overridable Function Clone() As Object Implements System.ICloneable.Clone
        Return New queue(Of valueT)(data)
    End Function

    'in fact, there should be no such function, but it's here, so leave them
    Public Function [erase](ByVal start As Int64, ByVal [end] As Int64) As Boolean
        While start < [end] AndAlso Not data.erase(start) Is Nothing
            start += 1
        End While

        Return start = [end]
    End Function

    Public Function [erase](ByVal start As Int64) As Boolean
        Return [erase](start, start + 1)
    End Function

    Public Sub New(ByVal that As queue(Of valueT))
        copy(data, that.data)
    End Sub

    Public Sub New(ByVal that As list(Of valueT))
        copy(data, that)
    End Sub

    Public Sub New()
        data = New list(Of valueT)()
    End Sub
End Class
