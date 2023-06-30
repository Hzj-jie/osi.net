
Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.device

Public Class device_pool_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(New should_stop_once())
    End Sub

    Private Class should_stop_once
        Inherits repeat_case_wrapper

        Public Sub New()
            MyBase.New(multithreading(New run_case(), 8), 1000)
        End Sub

        Private Class run_case
            Inherits [case]

            Private ReadOnly e As ManualResetEvent
            Private d As device_pool

            Public Sub New()
                e = New ManualResetEvent(False)
            End Sub

            Public Overrides Function prepare() As Boolean
                If MyBase.prepare() Then
                    e.Reset()
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Overrides Function run() As Boolean
                If multithreading_case_wrapper.thread_id = 0 Then
                    d = device_pool.new_for_test()
                    Dim i As atomic_int = Nothing
                    i = New atomic_int()
                    AddHandler d.closing, Sub()
                                              assertion.equal(i.increment(), 1)
                                          End Sub
                    assert(e.Set())
                Else
                    assert(e.WaitOne())
                    assert(Not d Is Nothing)
                    d.close()
                End If
                Return True
            End Function

            Protected Overrides Sub Finalize()
                e.Close()
                MyBase.Finalize()
            End Sub
        End Class
    End Class
End Class
