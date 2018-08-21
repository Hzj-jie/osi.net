
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

' Read-Write lock, allows multiple concurrent reads or single write.
Public NotInheritable Class rwlock
    Implements IDisposable

    Private ReadOnly zre As zero_reset_event
    Private l As lock_t

    Public Sub New()
        zre = New zero_reset_event()
    End Sub

    ' Claims a read operation.
    ' Returns the count of concurrent read operations currently performing.
    Public Function read_lock() As UInt32
        Dim r As UInt32 = 0
        l.wait()
        r = zre.increase()
        assert(r > 0)
        l.release()
        Return r
    End Function

    ' Finishes a read operation.
    ' Returns the count of concurrent read operations currently performing.
    Public Function read_unlock() As UInt32
        Dim r As UInt32 = 0
        l.wait()
        r = zre.decrease()
        l.release()
        Return r
    End Function

    ' Claims a write operation.
    Public Sub write_lock()
        While True
            l.wait()
            If zre.is_passing() Then
                Return
            End If
            l.release()
            zre.wait()
        End While
    End Sub

    ' Finishes a write operation.
    Public Sub write_unlock()
        l.release()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        zre.Dispose()
    End Sub

    Public Function scoped_read_lock() As IDisposable
        Return New auto_read_unlock(Me)
    End Function

    Public Function scoped_write_lock() As IDisposable
        Return New auto_write_unlock(Me)
    End Function

    Private NotInheritable Class auto_read_unlock
        Implements IDisposable

        Private ReadOnly l As rwlock

        Public Sub New(ByVal l As rwlock)
            assert(Not l Is Nothing)
            l.read_lock()
            Me.l = l
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            l.read_unlock()
        End Sub
    End Class

    Private NotInheritable Class auto_write_unlock
        Implements IDisposable

        Private ReadOnly l As rwlock

        Public Sub New(ByVal l As rwlock)
            assert(Not l Is Nothing)
            l.write_lock()
            Me.l = l
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            l.write_unlock()
        End Sub
    End Class
End Class
