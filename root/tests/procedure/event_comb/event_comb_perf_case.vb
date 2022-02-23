
Imports osi.root.procedure
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.lock

Friend Class event_comb_perf_case
    Inherits [case]

    Public Shared ReadOnly size As Int64

    Shared Sub New()
        size = 8192 * If(isdebugbuild(), 1, 8)
    End Sub

    Private ReadOnly finished As atomic_int
    Private ReadOnly ms As Int64

    Public Sub New(ByVal ms As Int64)
        Me.ms = ms
        Me.finished = New atomic_int()
    End Sub

    Private Function fake_work(ByVal e As Func(Of Boolean)) As Func(Of Boolean)
        assert(e IsNot Nothing)
        Return Function() As Boolean
                   fake_processor_work(rnd_int(0, ms))
                   Return waitfor(rnd_int(0, ms)) AndAlso
                          e()
               End Function
    End Function

    Private Function fake_others() As Func(Of Boolean)
        Return fake_work(AddressOf goto_next)
    End Function

    Private Function fake_last() As Func(Of Boolean)
        Return fake_work(Function() As Boolean
                             finished.increment()
                             Return goto_end()
                         End Function)
    End Function

    Private Function create_event_comb() As event_comb
        Dim ds() As Func(Of Boolean) = Nothing
        ReDim ds(rnd_int(1, 10))
        For i As Int32 = 0 To ds.Length() - 2
            ds(i) = fake_others()
        Next
        assert(ds.Length() > 0)
        ds(ds.Length() - 1) = fake_last()
        Return New event_comb(ds)
    End Function

    Public Overrides Function run() As Boolean
        Dim ec As event_comb = Nothing
        ec = New event_comb(Function() As Boolean
                                For i As Int32 = 0 To size - 1
                                    If Not waitfor(create_event_comb()) Then
                                        Return False
                                    End If
                                Next
                                Return goto_end()
                            End Function)
        Using New thread_lazy()
            assertion.is_true(async_sync(ec))
        End Using
        assertion.equal(+finished, size)
        Return True
    End Function
End Class
