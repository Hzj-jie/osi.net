
Imports System.Threading

Namespace slimlock
    Public Structure spinlock2
        Implements islimlock

        Private v As Int32

        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function

        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        Public Sub release() Implements islimlock.release
            Interlocked.Decrement(v)
        End Sub

        Public Sub wait() Implements islimlock.wait
            While Interlocked.Increment(v) <> 1
                Interlocked.Decrement(v)
            End While
        End Sub
    End Structure
End Namespace
