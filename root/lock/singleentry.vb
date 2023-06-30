
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Structure singleentry
    Private Const free As Int32 = DirectCast(Nothing, Int32)
    Private Const inuse As Int32 = free + 1
    Private state As Int32

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function in_use() As Boolean
        Return state = inuse
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function not_in_use() As Boolean
        Return state = free
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mark_in_use() As Boolean
        Return atomic.compare_exchange(state, inuse, free)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub release()
        assert(mark_not_in_use())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mark_not_in_use() As Boolean
        Return atomic.compare_exchange(state, free, inuse)
    End Function
End Structure
