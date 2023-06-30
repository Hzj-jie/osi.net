
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

Namespace slimlock
    Public Structure eventlock
        Implements islimlock

        Private are As AutoResetEvent

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub wait() Implements islimlock.wait
            atomic.create_if_nothing(are, True)
            assert(are.WaitOne())
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub release() Implements islimlock.release
            assert(are.Set())
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function

        <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2220:FinalizersShouldCallBaseClassFinalizer")>
        Protected Overrides Sub Finalize()
            are.Close()
        End Sub
    End Structure
End Namespace
