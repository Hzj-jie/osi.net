
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class zero_reset_event_test
    <test>
    Private Shared Sub should_allow_pass()
        Using e As zero_reset_event = New zero_reset_event(1)
            assert_begin(New event_comb(Function() As Boolean
                                            Return waitfor(100) AndAlso
                                               goto_next()
                                        End Function,
                                    Function()
                                        assertion.equal(e.decrease(), uint32_0)
                                        Return goto_end()
                                    End Function))
            assertion.is_true(e.wait(1000))
        End Using
    End Sub

    <test>
    Private Shared Sub should_block()
        Dim e As zero_reset_event = Nothing
        e = New zero_reset_event(0)
        assertion.equal(e.increase(), uint32_1)
        assertion.is_false(e.wait(1000))
        e.Dispose()
    End Sub

    <test>
    Private Shared Sub should_respect_init_state()
        Using e As zero_reset_event = New zero_reset_event(0)
            assertion.is_true(e.wait(0))
        End Using
        Using e As zero_reset_event = New zero_reset_event(1)
            assertion.is_false(e.wait(0))
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
