
Imports System.Threading
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class volatile_vs_memorybarrier_test
    Inherits commandline_specific_case_wrapper

    Private Const size As Int32 = 1024 * 1024 * 32
    Private Shared ReadOnly thread_count As Int32 = Environment.ProcessorCount() << 2

    Private Shared Function create(ByVal c As [case]) As [case]
        Return performance(multithreading(repeat(c, size), thread_count), times:=CULng(3))
    End Function

    Public Sub New()
        MyBase.New(chained(True,
                           create(New memory_barrier_case()),
                           create(New interlocked_exchange_case()),
                           create(New atomic_eva_atomic_read_case()),
                           create(New volatile_write_volatile_read_case()),
                           create(New atomic_eva_case()),
                           create(New volatile_write_case())))
    End Sub

    Private Class memory_barrier_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            'assume if using memory barrier, the cache of i will also be reloaded, so the performance is lower
            Thread.MemoryBarrier()
            j += 1
            Return True
        End Function
    End Class

    Private Class interlocked_exchange_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            Dim x As Int32 = 0
            Interlocked.Exchange(x, 0)
            j += 1
            Return True
        End Function
    End Class

    Private Class atomic_eva_atomic_read_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            atomic.eva(j, atomic.read(j) + 1)
            Return True
        End Function
    End Class

    Private Class volatile_write_volatile_read_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            Thread.VolatileWrite(j, Thread.VolatileRead(j) + 1)
            Return True
        End Function
    End Class

    Private Class atomic_eva_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            atomic.eva(j, j + 1)
            Return True
        End Function
    End Class

    Private Class volatile_write_case
        Inherits [case]

        Private j As Int64

        Public Overrides Function run() As Boolean
            Thread.VolatileWrite(j, j + 1)
            Return True
        End Function
    End Class
End Class
