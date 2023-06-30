
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.event
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class flip_event_test
    <test>
    Private Shared Sub cancellable()
        Dim to_high As UInt32 = 0
        Dim to_low As UInt32 = 0
        Dim f As flip_events.manual_flip_event = Nothing
        f = flip_events.manual()(flip_event.events.of(Sub()
                                                          to_high += uint32_1
                                                      End Sub,
                                                      Sub()
                                                          to_low += uint32_1
                                                      End Sub))
        f.raise_to_high()
        f.raise_to_low()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_1)
        f.cancel()
        f.raise_to_high()
        f.raise_to_low()
        assertion.equal(to_high, uint32_1)
        assertion.equal(to_low, uint32_1)
    End Sub

    Private Sub New()
    End Sub
End Class
