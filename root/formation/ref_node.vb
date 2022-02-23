
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics.CodeAnalysis
Imports osi.root.connector
Imports osi.root.constants

<SuppressMessage("", "BC42333")>
Public Class ref_node(Of T)
#Disable Warning BC42333
    Implements ICloneable,
               IComparable(Of ref_node(Of T)),
               IComparable(Of T),
               IComparable,
               IEquatable(Of ref_node(Of T)),
               ICloneable(Of ref_node(Of T))
#Enable Warning BC42333

    Private _data As T
    Private _p() As ref_node(Of T)

    Public Sub clear()
        _data = Nothing
        arrays.clear(_p)
    End Sub

    Public Sub New(ByVal ref_count As UInt32)
        initial_refs(ref_count)
    End Sub

    Public Sub New(ByVal ref_count As UInt32, ByVal newdata As T)
        initial_refs(ref_count)
        data() = newdata
    End Sub

    Private Sub initial_refs(ByVal ref_count As UInt32)
        assert(ref_count > uint32_0)
        ReDim _p(CInt(ref_count - uint32_1))
    End Sub

    Public Function ref_count() As UInt32
        Return array_size(_p)
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

    Protected Function available_index(ByVal index As UInt32) As Boolean
        Return index < ref_count() AndAlso index >= uint32_0
    End Function

    Public Property ref(ByVal index As UInt32) As ref_node(Of T)
        Get
            assert(available_index(index))
            Return _p(CInt(index))
        End Get
        Set(ByVal value As ref_node(Of T))
            assert(available_index(index))
            _p(CInt(index)) = value
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As ref_node(Of T) Implements ICloneable(Of ref_node(Of T)).Clone
        Dim rtn As ref_node(Of T) = Nothing
        rtn = allocate_instance_of(Me)
        rtn.data = _data

        rtn.initial_refs(ref_count())
        For i As UInt32 = uint32_0 To ref_count() - uint32_1
            rtn.ref(i) = ref(i)
        Next

        Return rtn
    End Function

    Public Shared Operator +(ByVal this As ref_node(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.data()
    End Operator

    Public Shared Widening Operator CType(ByVal this As ref_node(Of T)) As T
        Return +this
    End Operator

    Private Function CompareTo(ByVal that As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(_data, that)
    End Function

    Public Function CompareTo(ByVal that As ref_node(Of T)) As Int32 _
                             Implements IComparable(Of ref_node(Of T)).CompareTo
        Return CompareTo(+that)
    End Function

    'if that is an object, just think it's a ref_node,
    'for cases that use an Object to refer an instance.
    Public Function CompareTo(ByVal that As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of ref_node(Of T))(that, False))
    End Function

    'should call CompareTo(Object)
    Public Shared Operator =(ByVal this As ref_node(Of T), ByVal that As Object) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As ref_node(Of T), ByVal that As Object) As Boolean
        Return Not (this = that)
    End Operator

    'should call CompareTo(ref_node(Of T)
    Public Shared Operator =(ByVal this As ref_node(Of T), ByVal that As ref_node(Of T)) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As ref_node(Of T), ByVal that As ref_node(Of T)) As Boolean
        Return Not (this = that)
    End Operator

    'should call CompareTo(T)
    Public Shared Operator =(ByVal this As ref_node(Of T), ByVal that As T) As Boolean
        Return equal(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As ref_node(Of T), ByVal that As T) As Boolean
        Return Not (this = that)
    End Operator

    Public Shared Operator =(ByVal this As T, ByVal that As ref_node(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As T, ByVal that As ref_node(Of T)) As Boolean
        Return Not (this = that)
    End Operator

    Public Overloads Function Equals(ByVal other As ref_node(Of T)) As Boolean _
                                    Implements IEquatable(Of ref_node(Of T)).Equals
        Return CompareTo(other) = 0
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return CompareTo(obj) = 0
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(data())
    End Function

    Public Overrides Function GetHashCode() As Int32
        If data() IsNot Nothing Then
            Return data().GetHashCode()
        End If
        Return 0
    End Function
End Class
