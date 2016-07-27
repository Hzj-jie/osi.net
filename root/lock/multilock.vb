
Imports System.Threading
Imports osi.root.connector

Public Class multilock(Of lock_t As {Structure, slimlock.islimlock})
    Public Const defaultLockCount As Int64 = 8192
    Private locks() As lock_t

    Public Sub New(ByVal lockCount As Int64)
        ReDim locks(lockCount - 1)
    End Sub

    Public Sub New()
        Me.New(defaultLockCount)
    End Sub

    Private Function locksigning(Of T)(ByVal i As T) As UInt32
        Return signing(i) Mod locks.Length()
    End Function

    Public Sub lock(Of T)(ByVal i As T)
        locks(locksigning(i)).wait()
    End Sub

    Public Sub release(Of T)(ByVal i As T)
        locks(locksigning(i)).release()
    End Sub
End Class

Public Class multilock
    Inherits multilock(Of slimlock.monitorlock)

    Public Sub New(ByVal lockCount As Int64)
        MyBase.New(lockCount)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class
