
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

' An internal implementation for hashtable / hasharray / hashset.
Public Class hasher_node(Of T,
                            _HASHER As _to_uint32(Of T),
                            _EQUALER As _equaler(Of T),
                            _COMPARER As _comparer(Of T))
    Private Const uninitialized_hash_value As UInt32 = max_uint32
    Private Shared ReadOnly hasher As _HASHER = alloc(Of _HASHER)()
    Private Shared ReadOnly equaler As _equaler(Of T) = alloc(Of _EQUALER)()
    Private Shared ReadOnly comparer As _comparer(Of T) = alloc(Of _COMPARER)()
    Private ReadOnly v As T
    Private hash_value As UInt32

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v As T)
        Me.v = v
        Me.hash_value = uninitialized_hash_value
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get]() As T
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As hasher_node(Of T, _HASHER, _EQUALER, _COMPARER)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.get()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function hash_code() As UInt32
        If hash_value = uninitialized_hash_value Then
            hash_value = hasher(v)
        End If
        Return hash_value
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function equal_to(ByVal o As hasher_node(Of T, _HASHER, _EQUALER, _COMPARER)) As Boolean
#If DEBUG Then
        assert(Not o Is Nothing)
#End If
        If hash_code() <> o.hash_code() Then
            Return False
        End If
        Return equaler([get](), o.get())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function comparer_to(ByVal o As hasher_node(Of T, _HASHER, _EQUALER, _COMPARER)) As Int32
#If DEBUG Then
        assert(Not o Is Nothing)
#End If
        If hash_code() <> o.hash_code() Then
            Return hash_code().CompareTo(o.hash_code())
        End If
        Return comparer([get](), o.get())
    End Function
End Class
