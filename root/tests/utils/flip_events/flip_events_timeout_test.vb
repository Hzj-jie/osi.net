
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class flip_events_timeout_test
    <test>
    Private Shared Sub run()
        Dim to_high As UInt32 = 0
        Dim to_low As UInt32 = 0
        Dim f As flip_event = Nothing
        f = flip_events.timeout(1)(flip_event.events.of(Sub()
                                                            to_high += uint32_1
                                                        End Sub,
                                                        Sub()
                                                            to_low += uint32_1
                                                        End Sub))
        assert_equal(to_high, uint32_1)
        assert_true(lazy_sleep_wait_until(Function() As Boolean
                                              Return to_low = uint32_1
                                          End Function,
                                          100))
    End Sub

    Private Sub New()
    End Sub
End Class
