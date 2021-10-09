
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Namespace primitive
    ' Represent a reference in the heap, but cannot be directly used in primitive code. It's never serialized or
    ' deserialized.
    Public NotInheritable Class heap_ref
        Implements IComparable(Of heap_ref), IComparable

        Public ReadOnly high As UInt32
        Public ReadOnly low As UInt32

        Private Sub New(ByVal high As UInt32, ByVal low As UInt32)
            Me.high = high
            Me.low = low
        End Sub

        Public Shared Function of_address(ByVal i As UInt64) As heap_ref
            Return New heap_ref(CUInt(i >> CInt(bit_count_in_byte * sizeof_uint32)), CUInt(i And max_uint32))
        End Function

        Public Shared Function of_high(ByVal i As UInt32) As heap_ref
            Return New heap_ref(i, 0)
        End Function

        Public Function head_of_alloc() As Boolean
            Return low = 0
        End Function

        Public Function address() As UInt64
            Return (CULng(high) << CInt(bit_count_in_byte * sizeof_uint32)) Or low
        End Function

        Public Function shift(ByVal i As UInt32) As heap_ref
            If CULng(low) + i > max_uint32 Then
                executor_stop_error.throw(executor.error_type.heap_access_out_of_boundary)
                assert(False)
                Return Nothing
            End If
            Return New heap_ref(high, low + i)
        End Function

        Public Function shift(ByVal i As Int32) As heap_ref
            If i >= 0 Then
                Return shift(CUInt(i))
            End If
            If low + i >= 0 Then
                Return New heap_ref(high, CUInt(low + i))
            End If
            executor_stop_error.throw(executor.error_type.heap_access_out_of_boundary)
            assert(False)
            Return Nothing
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of heap_ref)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As heap_ref) As Int32 Implements IComparable(Of heap_ref).CompareTo
            Dim c As Int32 = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            If high <> other.high Then
                Return high.CompareTo(other.high)
            End If
            Return low.CompareTo(other.low)
        End Function
    End Class
End Namespace

