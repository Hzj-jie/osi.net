
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants

Namespace slimlock
    Public Structure sequentiallock
        Implements islimlock

        Private assign_token As Int32
        Private token As Int32
        Private are As AutoResetEvent

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub wait() Implements islimlock.wait
            atomic.create_if_nothing(are, False)
            Dim tokengot As Int32 = 0
            tokengot = Interlocked.Increment(assign_token) - 1
            While token < tokengot
                are.WaitOne()
                If token < tokengot Then
                    are.Set()
                    force_yield()
                End If
            End While
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub release() Implements islimlock.release
            Interlocked.Increment(token)
            are.Set()
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
