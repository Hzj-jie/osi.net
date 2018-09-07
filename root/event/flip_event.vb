
Option Explicit On
Option Infer Off
Option Strict On

' A signal to notice to_high and to_low events. One instance may trigger either event for multiple times.
Public Class flip_event
    Private ReadOnly to_high As Action
    Private ReadOnly to_low As Action

    Public Delegate Function [New](ByVal to_high As Action, ByVal to_low As Action) As flip_event

    Protected Sub New(ByVal to_high As Action, ByVal to_low As Action)
        Me.to_high = to_high
        Me.to_low = to_low
    End Sub

    Protected Sub raise_to_high()
        If Not to_high Is Nothing Then
            to_high()
        End If
    End Sub

    Protected Sub raise_to_low()
        If Not to_low Is Nothing Then
            to_low()
        End If
    End Sub
End Class
