
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atomic_uint64.vbp ----------
'so change atomic_uint64.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atomic_ulong.vbp ----------
'so change atomic_ulong.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atomic_uint.vbp ----------
'so change atomic_uint.vbp instead of this file



Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector

Public Class atomic_uint64
    Private i As Int64

    Public Sub New()
    End Sub

    Public Sub New(ByVal i As Int64)
        Me.i = i
    End Sub

    Public Sub New(ByVal i As UInt64)
        Me.i = uint64_int64(i)
    End Sub

    Public Function [get]() As UInt64
        Return int64_uint64(i)
    End Function

    Public Function add(ByVal i As UInt64) As UInt64
        Return int64_uint64(Interlocked.Add(Me.i, uint64_int64(i)))
    End Function

    Public Function add(ByVal i As Int64) As UInt64
        Return int64_uint64(Interlocked.Add(Me.i, i))
    End Function

    Public Function increment() As UInt64
        Return int64_uint64(Interlocked.Increment(i))
    End Function

    Public Function decrement() As UInt64
        Return int64_uint64(Interlocked.Decrement(i))
    End Function

    Public Function exchange(ByVal x As UInt64) As UInt64
        Return int64_uint64(Interlocked.Exchange(i, uint64_int64(x)))
    End Function

    Public Function compare_exchange(ByVal value As UInt64,
                                     ByVal comparand As UInt64) As UInt64
        Return int64_uint64(Interlocked.CompareExchange(i, uint64_int64(value), uint64_int64(comparand)))
    End Function

    Public Shared Operator +(ByVal i As atomic_uint64) As UInt64
        Return i.get()
    End Operator

    Public Shared Widening Operator CType(ByVal this As Int64) As atomic_uint64
        Return New atomic_uint64(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt64) As atomic_uint64
        Return New atomic_uint64(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As atomic_uint64) As UInt64
        Return +this
    End Operator
End Class

'finish atomic_uint.vbp --------
'finish atomic_ulong.vbp --------
'finish atomic_uint64.vbp --------
