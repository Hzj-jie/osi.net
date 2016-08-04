
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.streamer

Public Class pipe_commandline_specific_test2
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New pipe_test2(4 * 1024 * 1024))
    End Sub
End Class

Public Class pipe_test2
    Inherits multi_procedure_case_wrapper

    Public Sub New()
        Me.New(8 * 1024)
    End Sub

    Public Sub New(ByVal size As Int64)
        MyBase.New(repeat(New pipe_case2(), size), Environment.ProcessorCount() << 8)
    End Sub

    Private Class pipe_case2
        Inherits random_run_event_comb_case

        Private ReadOnly p As pipe(Of Int32)

        Public Sub New()
            MyBase.New()
            p = New pipe(Of Int32)(0, 0, False)
            insert_call(0.295, AddressOf push)
            insert_call(0.295, AddressOf sync_push)
            insert_call(0.2, AddressOf pop)
            insert_call(0.2, AddressOf sync_pop)
            insert_call(0.01, AddressOf clear)
        End Sub

        Public Overrides Function finish() As Boolean
            p.clear()
            Return MyBase.finish()
        End Function

        Private Function push() As event_comb
            Return p.push(rnd_int())
        End Function

        Private Function sync_push() As event_comb
            Return sync_async(Sub()
                                  p.sync_push(rnd_int())
                              End Sub)
        End Function

        Private Function pop() As event_comb
            Return event_comb.return_true(p.pop(Nothing))
        End Function

        Private Function sync_pop() As event_comb
            Return sync_async(Sub()
                                  p.sync_pop(0)
                              End Sub)
        End Function

        Private Function clear() As event_comb
            Return sync_async(Sub()
                                  p.clear()
                              End Sub)
        End Function
    End Class
End Class
