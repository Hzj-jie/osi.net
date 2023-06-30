
' A lock does nothing, to help to implement thread-safe / thread-unsafe version of class easily
Public Structure broken_lock
    Implements ilock

    Public Function held() As Boolean Implements ilock.held
        Return True
    End Function

    Public Function held_in_thread() As Boolean Implements ilock.held_in_thread
        Return True
    End Function

    Public Function can_cross_thread() As Boolean Implements ilock.can_cross_thread
        Return True
    End Function

    Public Function can_thread_owned() As Boolean Implements ilock.can_thread_owned
        Return True
    End Function

    Public Sub release() Implements ilock.release
    End Sub

    Public Sub wait() Implements ilock.wait
    End Sub
End Structure
