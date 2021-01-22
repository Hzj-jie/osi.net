
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public MustInherit Class timing_counter
    Implements IDisposable

    Private ReadOnly p As ref(Of Int64)
    Private ReadOnly s As Int64

    Public Sub New(ByVal p As ref(Of Int64))
        Me.p = p
        s = now()
    End Sub

    Protected MustOverride Function now() As Int64

    Public Sub Dispose() Implements IDisposable.Dispose
        eva(p, now() - s)
        GC.SuppressFinalize(Me)
    End Sub
End Class
