
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.lock.slimlock
Imports envs = osi.root.envs

Friend Class iqless_perf
    Inherits multithreading_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New(ByVal c As [case], ByVal thread_count As Int32)
        MyBase.New(repeat(c, (round() << 2) \ thread_count), thread_count)
    End Sub
End Class

Public MustInherit Class qless_perf
    Inherits performance_case_wrapper

    Private ReadOnly tc As Int32
    Private ReadOnly loop_ratio As Double

    Protected Sub New(ByVal c As [case], ByVal thread_count As Int32, ByVal loop_ratio As Double)
        MyBase.New(New iqless_perf(c, thread_count))
        Me.tc = thread_count
        Me.loop_ratio = loop_ratio
    End Sub

    Protected NotOverridable Overrides Function max_loops() As UInt64
        Return If(tc = Environment.ProcessorCount(), 1.5, 1) *
               If(isreleasebuild(), 10000000000, 4000000000) *
               tc *
               loop_ratio
    End Function
End Class

'for UTT
Public MustInherit Class qless_sl_perf
    Inherits qless_perf

    Protected Sub New(ByVal thread_count As Int32)
        MyBase.New(New qless_case(Of simplelock)(), thread_count, If(envs.virtual_machine, 1.8, 1.5))
    End Sub
End Class

Public Class qless_sl_perf_processor_count
    Inherits qless_sl_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount())
    End Sub
End Class

Public Class qless_sl_perf_two_processor_count
    Inherits qless_sl_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 1)
    End Sub
End Class

Public Class qless_sl_perf_four_processor_count
    Inherits qless_sl_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 2)
    End Sub
End Class

Public Class qless_sl_perf_fifty
    Inherits qless_sl_perf

    Public Sub New()
        MyBase.New(50)
    End Sub
End Class

Public Class qless_sl_perf_128
    Inherits qless_sl_perf

    Public Sub New()
        MyBase.New(128)
    End Sub
End Class

'for UTT
Public MustInherit Class qless2_perf
    Inherits qless_perf

    Protected Sub New(ByVal thread_count As Int32)
        MyBase.New(New qless2_case(), thread_count, If(envs.virtual_machine, 1.5, 1))
    End Sub
End Class

Public Class qless2_perf_processor_count
    Inherits qless2_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount())
    End Sub
End Class

Public Class qless2_perf_two_processor_count
    Inherits qless2_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 1)
    End Sub
End Class

Public Class qless2_perf_four_processor_count
    Inherits qless2_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 2)
    End Sub
End Class

Public Class qless2_perf_fifty
    Inherits qless2_perf

    Public Sub New()
        MyBase.New(50)
    End Sub
End Class

Public Class qless2_perf_128
    Inherits qless2_perf

    Public Sub New()
        MyBase.New(128)
    End Sub
End Class

'for UTT
Public MustInherit Class qless_ml_perf
    Inherits qless_perf

    Protected Sub New(ByVal thread_count As Int32)
        MyBase.New(New qless_case(Of monitorlock)(), thread_count, If(envs.virtual_machine, 1.8, 1.5))
    End Sub
End Class

Public Class qless_ml_perf_processor_count
    Inherits qless_ml_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount())
    End Sub
End Class

Public Class qless_ml_perf_two_processor_count
    Inherits qless_ml_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 1)
    End Sub
End Class

Public Class qless_ml_perf_four_processor_count
    Inherits qless_ml_perf

    Public Sub New()
        MyBase.New(Environment.ProcessorCount() << 2)
    End Sub
End Class

Public Class qless_ml_perf_fifty
    Inherits qless_ml_perf

    Public Sub New()
        MyBase.New(50)
    End Sub
End Class

Public Class qless_ml_perf_128
    Inherits qless_ml_perf

    Public Sub New()
        MyBase.New(128)
    End Sub
End Class
