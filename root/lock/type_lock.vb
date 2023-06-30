
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock.slimlock

Public Structure type_lock(Of T As islimlock, TYPE)
    Private Shared l As raw_type_lock(Of T, TYPE)

    Public Shared Sub release()
        l.release()
    End Sub

    Public Shared Sub wait()
        l.wait()
    End Sub

    Public Shared Function can_cross_thread() As Boolean
        Return l.can_cross_thread()
    End Function

    Public Shared Function can_thread_owned() As Boolean
        Return l.can_thread_owned()
    End Function
End Structure

Public Structure raw_type_lock(Of T As islimlock, TYPE)
    Implements islimlock

    Private Shared l As T

    Public Sub release() Implements islimlock.release
        l.release()
    End Sub

    Public Sub wait() Implements islimlock.wait
        l.wait()
    End Sub

    Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
        Return l.can_cross_thread()
    End Function

    Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
        Return l.can_thread_owned()
    End Function
End Structure

Public Structure type_lock(Of TYPE)
    Private Shared l As raw_type_lock(Of TYPE)

    Public Shared Sub release()
        l.release()
    End Sub

    Public Shared Sub wait()
        l.wait()
    End Sub

    Public Shared Function can_cross_thread() As Boolean
        Return l.can_cross_thread()
    End Function

    Public Shared Function can_thread_owned() As Boolean
        Return l.can_thread_owned()
    End Function
End Structure

Public Structure raw_type_lock(Of TYPE)
    Implements islimlock

    Private Shared l As monitorlock

    Public Sub release() Implements islimlock.release
        l.release()
    End Sub

    Public Sub wait() Implements islimlock.wait
        l.wait()
    End Sub

    Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
        Return l.can_cross_thread()
    End Function

    Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
        Return l.can_thread_owned()
    End Function
End Structure