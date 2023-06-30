
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants

Namespace slimlock
    Public Structure spinlock2
        Implements islimlock

        Private v As Int32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub release() Implements islimlock.release
            Interlocked.Decrement(v)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub wait() Implements islimlock.wait
            While Interlocked.Increment(v) <> 1
                Interlocked.Decrement(v)
            End While
        End Sub
    End Structure
End Namespace
