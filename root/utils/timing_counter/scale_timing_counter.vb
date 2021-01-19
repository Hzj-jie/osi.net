
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public MustInherit Class scale_timing_counter
    Inherits timing_counter

    Private ReadOnly scale As Int64
    Private l As Int64

    Public Sub New(ByVal p As ref(Of Int64), ByVal scale As Int64)
        MyBase.New(p)
        Me.scale = scale
    End Sub

    Protected MustOverride Function unscaled_now() As Int64

    Protected NotOverridable Overrides Function now() As Int64
        If l = 0 Then
            l = unscaled_now()
            assert(l > 0)
            Return 0
        Else
            Return (unscaled_now() - l) * scale
        End If
    End Function
End Class
