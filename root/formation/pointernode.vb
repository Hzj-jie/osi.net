
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics.CodeAnalysis
Imports osi.root.connector
Imports osi.root.constants

<SuppressMessage("Microsoft.Design", "BC42333")>
Public Class pointernode(Of T)
    Implements ICloneable, IComparable(Of pointernode(Of T)), IComparable(Of T), IComparable

    Private _data As T
    Private _p() As pointernode(Of T)

    Public Sub clear()
        _data = Nothing
        arrays.clear(_p)
    End Sub

    Public Sub New(ByVal pointer_count As UInt32)
        initial_pointers(pointer_count)
    End Sub

    Public Sub New(ByVal pointer_count As UInt32, ByVal newdata As T)
        initial_pointers(pointer_count)
        data() = newdata
    End Sub

    Private Sub initial_pointers(ByVal pointer_count As UInt32)
        assert(pointer_count > uint32_0)
        ReDim _p(CInt(pointer_count - uint32_1))
    End Sub

    Public Function pointer_count() As UInt32
        If _p Is Nothing Then
            Return 0
        Else
            Return CUInt(_p.Length())
        End If
    End Function

    Public Property data() As T
        Get
            Return _data
        End Get
        Set(ByVal value As T)
            emplace(copy_no_error(value))
        End Set
    End Property

    Public Sub emplace(ByVal value As T)
        _data = value
    End Sub

    Private Function available_index(ByVal index As UInt32) As Boolean
        Return index < pointer_count() AndAlso index >= uint32_0
    End Function

    Public Property pointer(ByVal index As UInt32) As pointernode(Of T)
        Get
            If available_index(index) Then
                Return _p(CInt(index))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As pointernode(Of T))
            If available_index(index) Then
                _p(CInt(index)) = value
            End If
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim rtn As pointernode(Of T) = Nothing
        rtn = allocate_instance_of(Me)
        rtn.data = _data

        rtn.initial_pointers(pointer_count())
        For i As UInt32 = uint32_0 To pointer_count() - uint32_1
            rtn.pointer(i) = pointer(i)
        Next

        Return rtn
    End Function

    Public Shared Operator +(ByVal this As pointernode(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.data()
        End If
    End Operator

    Public Shared Widening Operator CType(ByVal this As pointernode(Of T)) As T
        Return +this
    End Operator

    Private Function CompareTo(ByVal that As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(_data, that)
    End Function

    Public Function CompareTo(ByVal that As pointernode(Of T)) As Int32 _
                                                                Implements IComparable(Of pointernode(Of T)).CompareTo
#If RETIRED Then
        assert(Not that Is Nothing, "that should not be nothing after inherits to IComparable2(Of node(Of T)).")
#End If
        Return CompareTo(+that)
    End Function

    'if that is an object, just think it's a pointernode,
    'for cases that use an Object to refer an instance.
    Public Function CompareTo(ByVal that As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of pointernode(Of T))(that, False))
    End Function

    'should call CompareTo(Object)
    Public Shared Operator =(ByVal this As pointernode(Of T), ByVal that As Object) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As pointernode(Of T), ByVal that As Object) As Boolean
        Return Not (this = that)
    End Operator

    'should call CompareTo(pointernode(Of T)
    Public Shared Operator =(ByVal this As pointernode(Of T), ByVal that As pointernode(Of T)) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As pointernode(Of T), ByVal that As pointernode(Of T)) As Boolean
        Return Not (this = that)
    End Operator

    'should call CompareTo(T)
    Public Shared Operator =(ByVal this As pointernode(Of T), ByVal that As T) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As pointernode(Of T), ByVal that As T) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As T, ByVal that As pointernode(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As T, ByVal that As pointernode(Of T)) As Boolean
        Return Not (this = that)
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(Me, obj)
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(data())
    End Function

    Public Overrides Function GetHashCode() As Integer
        If Not data() Is Nothing Then
            Return data().GetHashCode()
        Else
            Return 0
        End If
    End Function
End Class
