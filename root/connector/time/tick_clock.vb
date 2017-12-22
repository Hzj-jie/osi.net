
Option Explicit On
Option Infer Off
Option Strict On

' A class to retrieve ticks. The public functions should not be expected to return data which can be converted to a
' human readable format. All the public functions are thread-safe.
Public Class tick_clock
    Private ReadOnly type_name As String

    Protected Sub New()
        type_name = Me.GetType().Name()
    End Sub

    Public Overridable Function ticks() As UInt64
        Return milliseconds_to_ticks(milliseconds())
    End Function

    Public Overridable Function milliseconds() As UInt64
        Return ticks_to_milliseconds(ticks())
    End Function

    Public Function seconds() As UInt64
        Return milliseconds_to_seconds(milliseconds())
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return strcat(type_name, " - Ticks: ", ticks())
    End Function
End Class

Public NotInheritable Class high_res_tick_clock
    Inherits tick_clock

    Public Shared ReadOnly instance As high_res_tick_clock

    Shared Sub New()
        instance = New high_res_tick_clock()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function ticks() As UInt64
        Dim r As Int64 = 0
        r = high_res_ticks_retriever.high_res_ticks()
        assert(r >= 0)
        Return CULng(r)
    End Function
End Class

Public NotInheritable Class low_res_tick_clock
    Inherits tick_clock

    Public Shared ReadOnly instance As low_res_tick_clock

    Shared Sub New()
        instance = New low_res_tick_clock()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function milliseconds() As UInt64
        Dim r As Int64 = 0
        r = low_res_ticks_retriever.low_res_milliseconds()
        assert(r >= 0)
        Return CULng(r)
    End Function
End Class

Public NotInheritable Class normal_res_tick_clock
    Inherits tick_clock

    Public Shared ReadOnly instance As normal_res_tick_clock

    Shared Sub New()
        instance = New normal_res_tick_clock()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function ticks() As UInt64
        Dim r As Int64 = 0
        r = DateTime.Now().Ticks()
        assert(r >= 0)
        Return CULng(r)
    End Function
End Class

Public NotInheritable Class default_tick_clock
    Inherits tick_clock

    Public Shared ReadOnly instance As default_tick_clock

    Shared Sub New()
        instance = New default_tick_clock()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function milliseconds() As UInt64
        Return low_res_tick_clock.instance.milliseconds()
    End Function
End Class