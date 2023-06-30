
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.event
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class flip_events_ref_counted_test
    <test>
    Private Shared Sub manual_start()
        Dim to_high As UInt32 = 0
        Dim to_low As UInt32 = 0
        Dim f As flip_events.ref_counted_flip_event = Nothing
        f = flip_events.ref_counted()(flip_event.events.of(Sub()
                                                               to_high += uint32_1
                                                           End Sub,
                                                           Sub()
                                                               to_low += uint32_1
                                                           End Sub))
        f.ref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.ref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.unref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.unref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_1)
        f.ref()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_1)
        f.unref()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_2)
    End Sub

    <test>
    Private Shared Sub auto_start()
        Dim to_high As UInt32 = 0
        Dim to_low As UInt32 = 0
        Dim f As flip_events.ref_counted_flip_event = Nothing
        f = flip_events.ref_counted(1)(flip_event.events.of(Sub()
                                                                to_high += uint32_1
                                                            End Sub,
                                                            Sub()
                                                                to_low += uint32_1
                                                            End Sub))
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.ref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.ref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.unref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.unref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        f.unref()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_1)
        f.ref()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_1)
        f.unref()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_2)
    End Sub

    Private Sub New()
    End Sub
End Class
