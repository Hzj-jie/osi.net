
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class cancellation_controller_test
    <test>
    Private Shared Sub should_cancel_others()
        Dim c As cancellation_controller = Nothing
        c = New cancellation_controller()
        Dim manual As UInt32 = 0
        Dim ref_counted As UInt32 = 0
        Dim timeout As UInt32 = 0
        Dim m As flip_events.manual_flip_event = Nothing
        Dim r As flip_events.ref_counted_flip_event = Nothing
        m = c.manual(Sub()
                         manual += uint32_1
                     End Sub)
        r = c.ref_counted(Sub()
                              ref_counted += uint32_1
                          End Sub)
        c.timeout(1000,
                  Sub()
                      timeout += uint32_1
                  End Sub)
        assertion.reference_equal(m, c.manual())
        c.manual().raise_to_low()
        r.unref()
        assertion.equal(manual, uint32_1)
        assertion.equal(ref_counted, uint32_0)
        assertion.equal(timeout, uint32_0)
        assertion.is_null(c.manual())
        assertion.is_null(c.ref_counted())
        assertion.is_null(c.timeout())
    End Sub

    <test>
    Private Shared Sub should_cancel_previous()
        Dim c As cancellation_controller = Nothing
        c = New cancellation_controller()
        Dim c1 As UInt32 = 0
        Dim c2 As UInt32 = 0
        Dim m As flip_events.manual_flip_event = Nothing
        m = c.manual(Sub()
                         c1 += uint32_1
                     End Sub)
        c.manual(Sub()
                     c2 += uint32_1
                 End Sub)
        assertion.not_reference_equal(m, c.manual())
        m.raise_to_low()
        c.manual().raise_to_low()
        assertion.equal(c1, uint32_0)
        assertion.equal(c2, uint32_1)
    End Sub

    <test>
    Private Shared Sub cancel_should_cancel_all()
        Dim c As cancellation_controller = Nothing
        c = New cancellation_controller()
        Dim manual As UInt32 = 0
        Dim ref_counted As UInt32 = 0
        Dim timeout As UInt32 = 0
        Dim m As flip_events.manual_flip_event = Nothing
        Dim r As flip_events.ref_counted_flip_event = Nothing
        m = c.manual(Sub()
                         manual += uint32_1
                     End Sub)
        r = c.ref_counted(Sub()
                              ref_counted += uint32_1
                          End Sub)
        c.timeout(1000,
                  Sub()
                      timeout += uint32_1
                  End Sub)
        c.cancel()
        assertion.is_null(c.manual())
        assertion.is_null(c.ref_counted())
        assertion.is_null(c.timeout())
        m.raise_to_low()
        r.unref()
        assertion.equal(manual, uint32_0)
        assertion.equal(ref_counted, uint32_0)
        assertion.equal(timeout, uint32_0)
    End Sub

    <test>
    Private Shared Sub bind_weak_ref_delegate()
        Dim c As cancellation_controller = Nothing
        c = New cancellation_controller()
        Dim o As Object = Nothing
        o = New Object()
        Dim r As Boolean = False
        Dim m As flip_events.manual_flip_event = Nothing
        m = c.manual(o,
                     Sub(ByVal i As Object)
                         assertion.reference_equal(i, o)
                         r = True
                     End Sub)
        m.raise_to_low()
        assertion.is_true(r)
    End Sub

    Private Sub New()
    End Sub
End Class
