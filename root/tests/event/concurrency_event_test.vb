
Imports osi.root.template
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.connector
Imports osi.root.utt

Public Class concurrency_event_test
    Inherits multithreading_case_wrapper

    Private Const repeat_count As Int32 = 32
    Private Const thread_count As Int32 = 16

    Public Sub New()
        MyBase.New(repeat(New concurrency_event_case(), repeat_count), thread_count)
    End Sub

    Private Class concurrency_event_case
        Inherits [case]

        Private Const concurrency_count As UInt32 = 4
        Private ReadOnly e As concurrency_event(Of _true)
        Private ReadOnly i As atomic_int
        Private ReadOnly c As atomic_int

        Public Sub New()
            e = New concurrency_event(Of _true)(concurrency_count)
            i = New atomic_int()
            c = New atomic_int()
            assert(e.concurrency < thread_count)
        End Sub

        Public Overrides Function run() As Boolean
            assertion.is_true(e.attach(Sub()
                                     c.increment()
                                     i.increment()
                                     assertion.more_or_equal(+i, 1)
                                     assertion.less_or_equal(CUInt(+i), concurrency_count)
                                     i.decrement()
                                 End Sub))
            Return True
        End Function

        Public Overrides Function prepare() As Boolean
            c.set(0)
            i.set(0)
            Return MyBase.prepare()
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(i.get(), 0)
            assertion.equal(c.get(), repeat_count * thread_count)
            Return MyBase.finish()
        End Function
    End Class
End Class
