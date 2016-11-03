
Imports System.Threading
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class reference_count_runner_test2
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim are As AutoResetEvent = Nothing
        are = New AutoResetEvent(False)
        Dim r As reference_count_runner = Nothing
        r = New reference_count_runner(Sub(ByVal this As reference_count_runner)
                                           assert(are.force_set())
                                           this.mark_started()
                                           sleep(10)
                                       End Sub)
        Dim t As Thread = Nothing
        t = New Thread(Sub()
                           While True
                               assert(are.wait())
                               r.mark_stopped()
                           End While
                       End Sub)
        t.Start()
        For i As Int32 = 0 To 1000
            assert_true(r.bind())
            assert_true(r.release())
        Next
        t.Abort()
        t.Join()
        are.Close()
        Return True
    End Function
End Class
