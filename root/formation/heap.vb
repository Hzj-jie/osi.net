
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' make sure T is comparable
Public Class heap(Of T As IComparable(Of T))
    Implements ICloneable, ICloneable(Of heap(Of T)), IComparable, IComparable(Of heap(Of T))

    Private Const head As Int64 = 0
    Private ReadOnly d As vector(Of T)

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
        End If
        If available_index(first) AndAlso available_index(second) Then
            _swap.swap(d(CUInt(first)), d(CUInt(second)))
            Return True
        End If
        Return False
    End Function

    Private Function up(ByVal index As Int64) As Int64
        If Not (available_index(index)) Then
            Return npos
        End If
        Dim parent As Int64 = npos
        While True
            parent = father(index)
            If Not (available_index(parent)) Then
                Exit While
            End If
            assert(index >= parent, "got an invalid parent ", parent, " index ", index)
            If index <= parent OrElse compare(d(CUInt(index)), d(CUInt(parent))) <= 0 Then
                Exit While
            End If
            assert(swap(index, parent), index, " or ", parent, " is not in heap.")
            index = parent
        End While

        assert(available_index(index), "up function makes an index not available.")
        Return index
    End Function

    Private Function down(ByVal index As Int64) As Int64
        If Not (available_index(index)) Then
            Return npos
        End If
        Dim min As Int64 = head
        Dim left As Int64 = head
        Dim right As Int64 = head
        While True
            min = index
            left = left_child(index)
            right = right_child(index)
            If available_index(left) Then
                If compare(d(CUInt(left)), d(CUInt(min))) > 0 Then
                    min = left
                End If
            End If
            If available_index(right) Then
                If compare(d(CUInt(right)), d(CUInt(min))) > 0 Then
                    min = right
                End If
            End If

            If min = index Then
                Exit While
            End If
            assert(swap(index, min), index, " or ", min, " is not in heap.")
            index = min
        End While

        assert(available_index(index), "down function makes an index not available.")
        Return index
    End Function

    Public Function available_index(ByVal index As Int64) As Boolean
        Return d.available_index(index)
    End Function

    Public Function empty() As Boolean
        Return d.empty()
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
        End If
        value = d(head)
        assert([erase](head), "erase(head) returns false.")
        Return True
    End Function

    Public Function pop(ByRef value As T) As Boolean
        Return pop_front(value)
    End Function

    Protected Sub pop_back(ByRef value As T)
        If empty() Then
            value = Nothing
        Else
            value = d.back()
            d.pop_back()
        End If
    End Sub

    Public Function insert(ByVal value As T) As Int64
        d.resize(d.size() + uint32_1)
        Return update(d.size() - 1, value)
    End Function

    Public Function push(ByVal value As T) As Int64
        Return insert(value)
    End Function

    Public Function take(ByVal index As Int64, ByRef value As T) As Boolean
        Return d.take(index, value)
    End Function

    Public Sub New()
        d = New vector(Of T)()
    End Sub

    <copy_constructor>
    Protected Sub New(ByVal d As vector(Of T))
        assert(Not d Is Nothing)
        Me.d = d
    End Sub

    Protected Sub New(ByVal v As heap(Of T))
        Me.New(assert_which.of(v).is_not_null().d)
    End Sub

    Public Overridable Sub clear()
        d.clear()
    End Sub

    Public Overridable Function update(ByVal index As Int64, ByVal value As T) As Int64
        Dim rtn As Int64
        If Not available_index(index) Then
            Return npos
        End If
        copy(d(CUInt(index)), value)
        rtn = up(index)
        If rtn = index Then
            rtn = down(index)
        End If
        Return rtn
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
        Return d.size()
    End Function

    Public Function CloneT() As heap(Of T) Implements ICloneable(Of heap(Of T)).Clone
        Return New heap(Of T)(copy_no_error(d))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
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
        If cmp <> object_compare_undetermined Then
            Return cmp
        End If
        assert(Not other Is Nothing)
        If size() <> other.size() Then
            Return CInt(size()) - CInt(other.size())
        End If
        Return compare(d, other.d)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of heap(Of T))(obj, False))
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(d)
    End Function
End Class
