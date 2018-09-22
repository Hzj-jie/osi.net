
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class event_comb_flip_event_test
    <test>
    Private Shared Sub repeat_when_high_with_stoppable_event_comb()
        Dim c As UInt32 = 0
        Dim e As event_comb = Nothing
        e = New event_comb(Function() As Boolean
                               c += uint32_1
                               Return goto_end()
                           End Function)
        Dim f As flip_events.manual_flip_event = Nothing
        f = e.repeat_when_high(flip_events.manual())
        assert_false(lazy_sleep_wait_until(Function() As Boolean
                                               Return c > uint32_0
                                           End Function,
                                           100))
        f.raise_to_high()
        assert_true(lazy_sleep_wait_until(Function() As Boolean
                                              Return c > uint32_0
                                          End Function,
                                          1000))
        f.raise_to_low()
        Dim now As UInt32 = 0
        now = c
        assert_false(lazy_sleep_wait_until(Function() As Boolean
                                               Return c <> now
                                           End Function,
                                           100))
    End Sub

    <test>
    Private Shared Sub repeat_when_high_with_non_stoppable_event_comb()
        Dim c As UInt32 = 0
        Dim e As event_comb = Nothing
        e = New event_comb(Function() As Boolean
                               c += uint32_1
                               Return waitfor_yield()
                           End Function)
        Dim f As flip_events.manual_flip_event = Nothing
        f = e.repeat_when_high(flip_events.manual())
        assert_false(lazy_sleep_wait_until(Function() As Boolean
                                               Return c > uint32_0
                                           End Function,
                                           100))
        f.raise_to_high()
        assert_true(lazy_sleep_wait_until(Function() As Boolean
                                              Return c > uint32_0
                                          End Function,
                                          1000))
        f.raise_to_low()
        Dim now As UInt32 = 0
        now = c
        assert_false(lazy_sleep_wait_until(Function() As Boolean
                                               Return c <> now
                                           End Function,
                                           100))
    End Sub

    Private Sub New()
    End Sub
End Class