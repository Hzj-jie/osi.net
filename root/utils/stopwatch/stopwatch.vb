﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class stopwatch
    Private Shared ReadOnly DELAY As Int64 = 0

    Shared Sub New()
        DELAY = counter.register_average_and_last_average("STOPWATCH_DELAY_MS")
    End Sub

    Public Shared Function push(ByVal e As [event]) As Boolean
        If e Is Nothing OrElse e.canceled() Then
            Return False
        End If
        Return queue_runner.push(AddressOf e.do)
    End Function

    Public Shared Function push(ByVal waitms As UInt32, ByVal d As Action) As [event]
        If d Is Nothing Then
            Return Nothing
        End If
        Dim rtn As [event] = Nothing
        rtn = New [event](waitms, d)
        assert(push(rtn))
        Return rtn
    End Function

    'the waitms is not consistant with other timeout settings,
    'since it's not reasonable to have a stopwatch event to be run in max_uint32 milliseconds later
    Private Shared Function _uint32(ByVal i As Int32) As UInt32
        Return If(i < 0, uint32_0, CUInt(i))
    End Function

    Private Shared Function _uint32(ByVal i As Int64) As UInt32
        Return If(i < 0, uint32_0, If(i > max_uint32, max_uint32, CUInt(i)))
    End Function

    Public Shared Function push(ByVal waitms As Int64, ByVal d As Action) As [event]
        Return push(_uint32(waitms), d)
    End Function

    Public Shared Function push(ByVal waitms As Int32, ByVal d As Action) As [event]
        Return push(_uint32(waitms), d)
    End Function

    Private Sub New()
    End Sub
End Class
