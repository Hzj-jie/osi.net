
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

' Notifies one or more threads when the count reaches zero.
Public NotInheritable Class zero_reset_event
    Implements IDisposable

    Private ReadOnly mre As ManualResetEvent
    Private c As Int32

    Public Sub New()
        Me.New(0)
    End Sub

    Public Sub New(ByVal init As Int32)
        mre = New ManualResetEvent(init = 0)
        c = init
    End Sub

    Public Function increase() As UInt32
        Dim r As Int32 = 0
        r = Interlocked.Increment(c)
        assert(r >= 1)
        If r = 1 Then
            mre.force_reset()
        End If
        Return CUInt(r)
    End Function

    Public Function decrease() As UInt32
        Dim r As Int32 = 0
        r = Interlocked.Decrement(c)
        assert(r >= 0)
        If r = 0 Then
            mre.force_set()
        End If
        Return CUInt(r)
    End Function

    Public Function is_zero() As Boolean
        assert(c >= 0)
        Return c = 0
    End Function

    Public Function is_passing() As Boolean
        Return is_zero()
    End Function

    Public Function is_blocking() As Boolean
        Return Not is_zero()
    End Function

    Public Function wait(ByVal ms As Int64) As Boolean
        Return mre.wait(ms)
    End Function

    Public Sub wait()
        assert(wait(npos))
    End Sub

    Public Sub wait_and_dispose()
        wait()
        Dispose()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        mre.Close()
        GC.SuppressFinalize(Me)
    End Sub
End Class
