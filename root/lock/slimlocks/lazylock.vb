
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Namespace slimlock
    Public Structure lazylock
        Implements islimlock

        Private se As singleentry

        Shared Sub New()
            yield()
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub release() Implements islimlock.release
            se.release()
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub wait() Implements islimlock.wait
            lazy_wait_when(Function(ByRef x) Not x.mark_in_use(), se)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function
    End Structure
End Namespace
