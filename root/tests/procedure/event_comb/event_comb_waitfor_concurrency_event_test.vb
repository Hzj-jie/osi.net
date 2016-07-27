
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.utils
Imports utt = osi.root.utt

Public Class event_comb_waitfor_concurrency_event_test
    Inherits multi_procedure_case_wrapper

    Private Const repeat_count As Int32 = 1024
    Private Shadows Const procedure_count As Int32 = 256

    Public Sub New()
        MyBase.New(repeat(New event_comb_waitfor_concurrency_event_case(), repeat_count), procedure_count)
    End Sub

    Private Class event_comb_waitfor_concurrency_event_case
        Inherits utt.event_comb_case

        Private Const concurrency_count As UInt32 = 64
        Private ReadOnly e As concurrency_event(Of _false)
        Private ReadOnly i As atomic_int
        Private ReadOnly c As atomic_int

        Shared Sub New()
            assert(concurrency_count < procedure_count)
        End Sub

        Public Sub New()
            e = New concurrency_event(Of _false)(concurrency_count)
            i = New atomic_int()
            c = New atomic_int()
            assert(e.concurrency < procedure_count)
        End Sub

        Private Sub assert_concurrency()
            assert(Not i Is Nothing)
            assert_more(+i, 0)
            assert_less_or_equal(CUInt(+i), concurrency_count)
        End Sub

        Public Overrides Function create() As event_comb
            Return New event_comb(Function() As Boolean
                                      Return waitfor(e) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      c.increment()
                                      i.increment()
                                      assert_concurrency()
                                      Return If(rnd_bool(), waitfor_yield(), waitfor_nap()) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assert_concurrency()
                                      i.decrement()
                                      e.release()
                                      Return goto_end()
                                  End Function)
        End Function

        Public Overrides Function prepare() As Boolean
            i.set(0)
            c.set(0)
            Return MyBase.prepare()
        End Function

        Public Overrides Function finish() As Boolean
            assert_equal(i.get(), 0)
            assert_equal(c.get(), procedure_count * repeat_count)
            Return MyBase.finish()
        End Function
    End Class
End Class
