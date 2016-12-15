
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector
Imports osi.service.transmitter

Partial Public Class shared_component_test
    Private Class outgoing_test
        Inherits [case]

        Private ReadOnly c As collection

        Public Sub New()
            c = New collection()
        End Sub

        Public Overrides Function run() As Boolean
            Dim s As shared_component(Of UInt16, UInt16, component, Int32, parameter) = Nothing
            s = shared_component.[New](Of UInt16, UInt16, component, Int32, parameter) _
                                      (New parameter(), c, emplace_make_const_pair(Of UInt16, UInt16)(100, 100), 200)
            Dim ec As event_comb = Nothing
            Dim p As pointer(Of Int32) = Nothing
            p = New pointer(Of Int32)()
            assert_true(async_sync(s.receiver.receive(p)))
            assert_equal(+p, 200)  ' The initial data
            For i As Int32 = 0 To 1000
                s.component().push(i, 100, 100)
                assert_true(async_sync(s.receiver.receive(p)))
                assert_equal(+p, i)
            Next
            s.component().push(100, 101, 100)
            assert_false(async_sync(s.receiver.sense(1000)))
            s.component().push(100, 100, 101)
            assert_false(async_sync(s.receiver.sense(1000)))
            s.dispose()
            assert_false(valuer.get(Of ref_instance(Of component)) _
                                   (s, binding_flags.instance_private, "component_ref").referred())
            assert_false(valuer.get(Of dispenser(Of Int32, const_pair(Of UInt16, UInt16))) _
                                   (s, binding_flags.instance_private, "dispenser").binding())
            Return True
        End Function
    End Class
End Class
