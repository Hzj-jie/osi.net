
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Public Structure monitorlock
    Implements ilock

    Private obj As Object
    Private in_use As Int32

    Shared Sub New()
        assert(DirectCast(Nothing, Object) Is Nothing)
    End Sub

    Public Sub New(ByVal i As Object)
        assert(Not i Is Nothing)
        Me.obj = i
    End Sub

    Public Sub wait() Implements ilock.wait
        atomic.create_if_nothing(obj)
        Monitor.Enter(obj)
        in_use += 1
    End Sub

    Public Sub release() Implements ilock.release
        in_use -= 1
#If DEBUG Then
        assert(in_use >= 0)
#End If
        Monitor.Exit(obj)
    End Sub

    Public Function can_thread_owned() As Boolean Implements ilock.can_thread_owned
        Return True
    End Function

    Public Function can_cross_thread() As Boolean Implements ilock.can_cross_thread
        Return False
    End Function

    Public Function held() As Boolean Implements ilock.held
        Return in_use > 0
    End Function

    Public Function held_in_thread() As Boolean Implements ilock.held_in_thread
        If held() Then
            If Monitor.TryEnter(obj) Then
                Dim rtn As Boolean = False
                rtn = held()
                Monitor.Exit(obj)
                Return rtn
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
End Structure
