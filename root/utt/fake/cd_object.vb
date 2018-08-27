﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock

Public Class cd_object(Of T)
    Implements IDisposable

    Private Shared ReadOnly c As atomic_int
    Private Shared ReadOnly d As atomic_int
    Private Shared ReadOnly f As atomic_int

    Shared Sub New()
        c = New atomic_int()
        d = New atomic_int()
        f = New atomic_int()
    End Sub

    Public Shared Sub reset()
        c.set(0)
        d.set(0)
        f.set(0)
    End Sub

    Public Shared Function constructed() As UInt32
        Return CUInt(+c)
    End Function

    Public Shared Function next_id() As UInt32
        Return constructed()
    End Function

    Public Shared Function disposed() As UInt32
        Return CUInt(+d)
    End Function

    Public Shared Function destructed() As UInt32
        Return CUInt(+f)
    End Function

    Public Shared Function finalized() As UInt32
        Return destructed()
    End Function

    Public ReadOnly id As UInt32

    Public Sub New()
        id = CUInt(c.increment() - 1)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        d.increment()
    End Sub

    Protected Overrides Sub Finalize()
        f.increment()
        MyBase.Finalize()
    End Sub
End Class
