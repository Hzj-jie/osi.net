
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Namespace slimlock
    Public Structure monitorlock
        Implements islimlock

        'the obj is created in atomic.create_if_nothing
        Private obj As Object

        Shared Sub New()
            assert(DirectCast(Nothing, Object) Is Nothing)
        End Sub

        Public Sub New(ByVal i As Object)
            assert(Not i Is Nothing)
            Me.obj = i
        End Sub

        Public Sub wait() Implements islimlock.wait
            atomic.create_if_nothing(obj)
            Monitor.Enter(obj)
        End Sub

        Public Sub release() Implements islimlock.release
            Monitor.Exit(obj)
        End Sub

        Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
            Return True
        End Function

        Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
            Return False
        End Function
    End Structure
End Namespace
