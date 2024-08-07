
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Class multilock(Of lock_t As {Structure, slimlock.islimlock})
    Private Const default_lock_count As UInt32 = 257
    Private ReadOnly locks() As lock_t

    Public Sub New(ByVal lock_count As UInt32)
        assert(lock_count > 0)
        ReDim locks(CInt(lock_count - uint32_1))
    End Sub

    Public Sub New()
        Me.New(default_lock_count)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function lock_signing(Of T)(ByVal i As T) As UInt32
        Return signing(i) Mod array_size(locks)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub lock(Of T)(ByVal i As T)
        locks(CInt(lock_signing(i))).wait()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub release(Of T)(ByVal i As T)
        locks(CInt(lock_signing(i))).release()
    End Sub
End Class

Public NotInheritable Class multilock
    Inherits multilock(Of slimlock.monitorlock)

    Public Sub New(ByVal lock_count As UInt32)
        MyBase.New(lock_count)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub
End Class
