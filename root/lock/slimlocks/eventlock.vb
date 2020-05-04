
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector

Namespace slimlock
    Public Structure eventlock
        Implements islimlock

        Private are As AutoResetEvent

        Public Sub wait() Implements islimlock.wait
            atomic.create_if_nothing(are, True)
            assert(are.WaitOne())
        End Sub

        Public Sub release() Implements islimlock.release
            assert(are.Set())
        End Sub

        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function

        Protected Overrides Sub Finalize()
            are.Close()
        End Sub
    End Structure
End Namespace
