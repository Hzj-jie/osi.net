
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public Class event_comb
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function not_started() As Boolean
        Return debug_locked(AddressOf _not_started)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function started() As Boolean
        Return Not not_started()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function working() As Boolean
        Return debug_locked(AddressOf _working)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function ending() As Boolean
        Return debug_locked(AddressOf _ending)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [end]() As Boolean
        Return debug_locked(AddressOf _end)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function callback_resume_ready() As Boolean
        Return debug_locked(AddressOf _callback_resume_ready)
    End Function
End Class
