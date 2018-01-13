
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class tick_clock
    Private NotInheritable Class high_res_tick_clock
        Inherits tick_clock

        Public Overrides Function ticks() As UInt64
            Dim r As Int64 = 0
            r = high_res_ticks_retriever.high_res_ticks()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class

    Private NotInheritable Class low_res_tick_clock
        Inherits tick_clock

        Public Overrides Function milliseconds() As UInt64
            Dim r As Int64 = 0
            r = low_res_ticks_retriever.low_res_milliseconds()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class

    Private NotInheritable Class normal_res_tick_clock
        Inherits tick_clock

        Public Overrides Function ticks() As UInt64
            Dim r As Int64 = 0
            r = DateTime.Now().Ticks()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class
End Class
