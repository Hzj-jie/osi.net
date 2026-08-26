
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class tick_clock
    Private NotInheritable Class high_res_tick_clock
        Inherits tick_clock

        Public Overrides Function ticks() As UInt64
            Dim r As Int64 = high_res_ticks_retriever.ticks()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class

    Private NotInheritable Class low_res_tick_clock
        Inherits tick_clock

        Public Overrides Function milliseconds() As UInt64
            Dim r As Int64 = low_res_ticks_retriever.milliseconds()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class

    Private NotInheritable Class normal_res_tick_clock
        Inherits tick_clock

        Public Overrides Function ticks() As UInt64
            Dim r As Int64 = DateTime.Now().Ticks()
            assert(r >= 0)
            Return CULng(r)
        End Function
    End Class
End Class
