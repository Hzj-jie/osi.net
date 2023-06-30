
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

Namespace slimlock
    Public Structure monitorlock
        Implements islimlock

        'the obj is created in atomic.create_if_nothing
        Private obj As Object

        Public Sub New(ByVal i As Object)
            assert(Not i Is Nothing)
            Me.obj = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub wait() Implements islimlock.wait
            atomic.create_if_nothing(obj)
            Monitor.Enter(obj)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub release() Implements islimlock.release
            Monitor.Exit(obj)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return False
        End Function
    End Structure
End Namespace
