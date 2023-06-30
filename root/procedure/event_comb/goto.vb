
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _goto
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [goto](ByVal [step] As Int32) As Boolean
        Return event_comb.goto([step])
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goback() As Boolean
        Return event_comb.goback()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goto_prev() As Boolean
        Return event_comb.goto_prev()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goto_end() As Boolean
        Return event_comb.goto_end()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goto_next() As Boolean
        Return event_comb.goto_next()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goto_last() As Boolean
        Return event_comb.goto_last()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function goto_begin() As Boolean
        Return event_comb.goto_begin()
    End Function
End Module
