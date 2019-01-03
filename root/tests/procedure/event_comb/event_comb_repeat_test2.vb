
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class event_comb_repeat_test2
    <test>
    Private Shared Sub shared_repeat_should_repeat()
        Dim c As Int32 = 0
        async_sync(event_comb.repeat(100, Function(ByVal i As UInt32) As event_comb
                                              Return event_comb.succeeded(Sub()
                                                                              c += 1
                                                                          End Sub)
                                          End Function))
        assertion.equal(c, 100)
    End Sub

    <test>
    Private Shared Sub shared_repeat_should_stop_after_failure()
        Dim c As Int32 = 0
        async_sync(event_comb.repeat(100, Function(ByVal i As UInt32) As event_comb
                                              Return event_comb.one_step(Function() As Boolean
                                                                             c += 1
                                                                             Return False
                                                                         End Function)
                                          End Function))
        assertion.equal(c, 1)
    End Sub

    Private Sub New()
    End Sub
End Class
