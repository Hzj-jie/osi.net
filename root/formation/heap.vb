﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' make sure T is comparable
Public Class heap(Of T As IComparable(Of T))
    Implements ICloneable, IComparable, IComparable(Of heap(Of T))

    Public Const head As Int64 = 0
    Public Const npos As Int64 = -1
    Public Const data_seg_size As Int64 = 1024
    Private _data As vector(Of T)

    Private Function data() As vector(Of T)
        assert(Not _data Is Nothing)
        Return _data
    End Function

    Private Shared Function father(ByVal index As Int64) As Int64
        Return (index - 1) >> 1
    End Function

    Private Shared Function left_child(ByVal index As Int64) As Int64
        Return (index << 1) + 1
    End Function

    Private Shared Function right_child(ByVal index As Int64) As Int64
        Return left_child(index) + 1
    End Function

    Protected Overridable Function swap(ByVal first As Int64, ByVal second As Int64) As Boolean
        If first = second Then
            Return available_index(first)
        ElseIf available_index(first) AndAlso available_index(second) Then
            _swap.swap(data()(CUInt(first)), data()(CUInt(second)))
            Return True
        Else
            Return False
        End If
    End Function

    Private Function up(ByVal index As Int64) As Int64
        If Not (available_index(index)) Then
            Return npos
        Else
            Dim parent As Int64 = npos
            While (True)
                parent = father(index)
                If Not (available_index(parent)) Then
                    Exit While
                Else
                    debug_assert(index >= parent _
                                , "got an invalid parent " + Convert.ToString(parent) _
                                + " index " + Convert.ToString(index))
                    If index > parent AndAlso compare(data()(CUInt(index)), data()(CUInt(parent))) > 0 Then
                        debug_assert(swap(index, parent) _
                                    , Convert.ToString(index) + " or " _
                                    + Convert.ToString(parent) + " is not in heap.")
                        index = parent
                    Else
                        Exit While
                    End If
                End If
            End While

            debug_assert(available_index(index), "up function makes an index not available.")
            Return index
        End If
    End Function

    Private Function down(ByVal index As Int64) As Int64
        If Not (available_index(index)) Then
            Return npos
        Else
            Dim min As Int64 = head
            Dim left As Int64 = head
            Dim right As Int64 = head
            While (True)
                min = index
                left = left_child(index)
                right = right_child(index)
                If available_index(left) Then
                    If compare(data()(CUInt(left)), data()(CUInt(min))) > 0 Then
                        min = left
                    End If
                End If
                If available_index(right) Then
                    If compare(data()(CUInt(right)), data()(CUInt(min))) > 0 Then
                        min = right
                    End If
                End If

                If min <> index Then
                    debug_assert(swap(index, min) _
                                , Convert.ToString(index) + " or " + Convert.ToString(min) + " is not in heap.")
                    index = min
                Else
                    Exit While
                End If
            End While

            debug_assert(available_index(index), "down function makes an index not available.")
            Return index
        End If
    End Function

    Public Function available_index(ByVal index As Int64) As Boolean
        Return data().available_index(index)
    End Function

    Public Function empty() As Boolean
        Return data().empty()
    End Function

    Public Overridable Function [erase](ByVal index As Int64) As Boolean
        If Not available_index(index) Then
            Return False
        End If
        Dim tail As T = Nothing
        pop_back(tail)
        If index <> size() Then
            update(index, tail)
        End If

        Return True
    End Function

    Public Function pop_front(ByRef value As T) As Boolean
        If empty() Then
            value = Nothing
            Return False
        Else
            value = data()(head)
            debug_assert([erase](head), "erase(head) returns false.")
            Return True
        End If
    End Function

    Public Function pop(ByRef value As T) As Boolean
        Return pop_front(value)
    End Function

    Protected Sub pop_back(ByRef value As T)
        If empty() Then
            value = Nothing
        Else
            value = data().back()
            data().pop_back()
        End If
    End Sub

    Public Function insert(ByVal value As T) As Int64
        data().resize(data().size() + uint32_1)
        Return update(data().size() - 1, value)
    End Function

    Public Function push(ByVal value As T) As Int64
        Return insert(value)
    End Function

    Public Function take(ByVal index As Int64, ByRef value As T) As Boolean
        Return data().take(index, value)
    End Function

    Public Sub New()
        _data = New vector(Of T)()
    End Sub

    Public Overridable Sub clear()
        data().clear()
    End Sub

    Public Overridable Function update(ByVal index As Int64, ByVal value As T) As Int64
        Dim rtn As Int64
        If available_index(index) Then
            copy(data()(CUInt(index)), value)
            rtn = up(index)
            If rtn = index Then
                rtn = down(index)
            End If
            Return rtn
        Else
            Return npos
        End If
    End Function

    Default Public Property at(ByVal index As Int64) As T
        Get
            Dim value As T = Nothing
            take(index, value)
            Return value
        End Get
        Set(ByVal value As T)
            update(index, value)
        End Set
    End Property

    Public Function size() As Int64
        Return data().size()
    End Function

    Public Function clone() As Object Implements ICloneable.Clone
        Dim rtn As heap(Of T)
        rtn = allocate_instance_of(Me)
        copy(rtn._data, data())
        Return rtn
    End Function

    Public Shared Operator <>(ByVal this As heap(Of T), ByVal that As heap(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As heap(Of T), ByVal that As heap(Of T)) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Function CompareTo(ByVal other As heap(Of T)) As Int32 _
                             Implements IComparable(Of heap(Of T)).CompareTo
        Dim cmp As Int32 = 0
        cmp = object_compare(Me, other)
        If cmp = object_compare_undetermined Then
            assert(Not other Is Nothing)
            If size() <> other.size() Then
                Return CInt(size()) - CInt(other.size())
            Else
                Return compare(data(), other.data())
            End If
        Else
            Return cmp
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of heap(Of T))(obj, False))
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(data())
    End Function
End Class
