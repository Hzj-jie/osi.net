
Imports System.Threading
Imports osi.root.envs
Imports osi.root.connector

Namespace slimlock
    Public Structure sequentiallock
        Implements islimlock

        Private assign_token As Int32
        Private token As Int32
        Private are As AutoResetEvent

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

        Public Sub release() Implements islimlock.release
            Interlocked.Increment(token)
            are.Set()
        End Sub

        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return True
        End Function
    End Structure
End Namespace
