
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' A signal to notice to_high and to_low events. One instance may trigger either event for multiple times.
Public Class flip_event
    Private ReadOnly to_high As Action
    Private ReadOnly to_low As Action

    Public NotInheritable Class events
        Public to_high As Action
        Public to_low As Action

        Public Function with_to_high(ByVal v As Action) As events
            to_high = v
            Return Me
        End Function

        Public Function with_to_low(ByVal v As Action) As events
            to_low = v
            Return Me
        End Function

        Public Shared Function [of](ByVal to_high As Action, ByVal to_low As Action) As events
            Return New events() With {.to_high = to_high, .to_low = to_low}
        End Function

        Public Shared Function of_to_low(ByVal to_low As Action) As events
            Return New events() With {.to_low = to_low}
        End Function
    End Class

    Public Delegate Function [New](Of T As flip_event)(ByVal e As events) As T

    Protected Sub New(ByVal e As events)
        assert(Not e Is Nothing)
        Me.to_high = e.to_high
        Me.to_low = e.to_low
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
