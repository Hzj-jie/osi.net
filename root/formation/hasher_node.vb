
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

' An internal implementation for hashtable / hasharray / hashset.
Public NotInheritable Class hasher_node(Of T)
    Private Const uninitialized_hash_value As UInt32 = max_uint32
    Private ReadOnly v As T
    Private ReadOnly hasher As _to_uint32(Of T)
    Private hash_value As UInt32

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v As T, ByVal hasher As _to_uint32(Of T))
        assert(Not hasher Is Nothing)
        Me.v = v
        Me.hasher = hasher
        Me.hash_value = uninitialized_hash_value
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get]() As T
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As hasher_node(Of T)) As T
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
End Class
