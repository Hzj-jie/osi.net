
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.event
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class flip_events_combiner_test
    <test>
    Private Shared Sub run()
        Dim to_high As UInt32 = 0
        Dim to_low As UInt32 = 0
        Dim f As flip_events.combiner_flip_event = Nothing
        f = flip_events.combiner()(flip_event.events.of(Sub()
                                                            to_high += uint32_1
                                                        End Sub,
                                                        Sub()
                                                            to_low += uint32_1
                                                        End Sub))
        Dim m1 As flip_events.manual_flip_event = Nothing
        Dim m2 As flip_events.manual_flip_event = Nothing
        m1 = f.attach(flip_events.manual())
        m2 = f.attach(flip_events.manual())

        m1.raise_to_high()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        m2.raise_to_high()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        m1.raise_to_low()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_0)
        m2.raise_to_low()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_1)

        m1.raise_to_high()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_1)
        m1.raise_to_low()
        assertion.equal(to_high, uint32_2)
        assertion.equal(to_low, uint32_2)
        m2.raise_to_high()
        assertion.equal(to_high, uint32_3)
        assertion.equal(to_low, uint32_2)
        m2.raise_to_low()
        assertion.equal(to_high, uint32_3)
        assertion.equal(to_low, uint32_3)
    End Sub

    Private Sub New()
    End Sub
End Class
