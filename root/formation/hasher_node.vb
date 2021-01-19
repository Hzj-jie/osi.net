
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

' An internal implementation for hashtable / hasharray / hashset.
Public Structure hasher_node(Of T)
    Private Const uninitialized_hash_value As UInt32 = max_uint32
    Private ReadOnly v As T
    Private ReadOnly hasher As _to_uint32(Of T)
    Private ReadOnly equaler As _equaler(Of T)
    Private hash_value As UInt32

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v As T, ByVal hasher As _to_uint32(Of T), ByVal equaler As _equaler(Of T))
#If DEBUG Then
        assert(Not hasher Is Nothing)
        assert(Not equaler Is Nothing)
#End If
        Me.v = v
        Me.hasher = hasher
        Me.hash_value = uninitialized_hash_value
        Me.equaler = equaler
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function uninitialized() As Boolean
#If DEBUG Then
        assert((hasher Is Nothing) = (equaler Is Nothing))
#End If
        Return hasher Is Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get]() As T
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As hasher_node(Of T)) As T
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
    Public Function equal_to(ByVal o As hasher_node(Of T)) As Boolean
#If 0 Then
        If hash_code() <> o.hash_code() Then
            Return False
        End If
#End If
        Return equaler([get](), o.get())
    End Function
End Structure