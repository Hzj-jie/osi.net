
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public Class dataprovider_datawatcher
    Inherits trigger_datawatcher

    Public Enum trigger_rule
        all
        one
    End Enum

    Public Sub New(ByVal rule As trigger_rule, ByVal ParamArray dps() As idataprovider)
        assert(Not isemptyarray(dps))
        Dim wdps() As weak_pointer(Of idataprovider) = Nothing
        wdps = weak_pointers.of(dps)
        For i As Int32 = 0 To dps.Length() - 1
            assert(Not dps(i) Is Nothing)
            dps(i).register_update_event(Me, Sub(x) x.rule_trigger(rule, wdps))
        Next
    End Sub

    Private Sub rule_trigger(ByVal rule As trigger_rule, ByVal dps() As weak_pointer(Of idataprovider))
        Dim last_trigger_time_ticks As Int64
        assert(Not isemptyarray(dps))
        If rule = trigger_rule.one OrElse dps.Length() = 1 Then
            trigger()
        Else
            For i As Int32 = 0 To dps.Length() - 1
                Dim p As idataprovider = Nothing
                p = (+(dps(i)))
                If p Is Nothing OrElse
                   Not p.valid() OrElse
                   p.last_updated_ticks() < last_trigger_time_ticks Then
                    Return
                End If
            Next
            For i As Int32 = 0 To dps.Length() - 1
                Dim p As idataprovider = Nothing
                p = (+(dps(i)))
                If Not p Is Nothing AndAlso
                   p.valid() AndAlso
                   p.last_updated_ticks() > last_trigger_time_ticks Then
                    last_trigger_time_ticks = p.last_updated_ticks()
                End If
            Next
            trigger()
        End If
    End Sub
End Class
