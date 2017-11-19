
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector
Imports osi.service.sharedtransmitter
Imports osi.service.transmitter

Partial Public Class sharedtransmitter_test
    Private Class outgoing_test
        Inherits [case]

        Private ReadOnly c As collection

        Public Sub New()
            MyBase.New()
            c = New collection()
        End Sub

        Public Overrides Function reserved_processors() As Int16
            Return 1
        End Function

        Public Overrides Function run() As Boolean
            Dim param As parameter = Nothing
            param = New parameter(False)
            Dim s As sharedtransmitter(Of Byte, Byte, component, Int32, parameter) = Nothing
            s = sharedtransmitter(Of Byte, Byte, component, Int32, parameter).creator.[New]().
                        with_parameter(param).
                        with_local_port(param.local_port).
                        with_remote(emplace_make_const_pair(Of Byte, Byte)(100, 100)).
                        with_data(200).
                        with_collection(c).
                        with_functor(Of functor)().
                        create()
            Dim p As pointer(Of Int32) = Nothing
            p = New pointer(Of Int32)()
            assert_true(async_sync(s.receiver.receive(p)))
            assert_equal(+p, 200)  ' The initial data
            For i As Int32 = 0 To 1000
                s.component().receive(i, 100, 100)
                assert_true(async_sync(s.receiver.receive(p)))
                assert_equal(+p, i)
            Next
            s.component().receive(100, 101, 100)
            assert_false(async_sync(s.receiver.sense(1000)))
            s.component().receive(100, 100, 101)
            assert_false(async_sync(s.receiver.sense(1000)))
            s.dispose()
            assert_false(valuer.get(Of ref_instance(Of component)) _
                                   (s, binding_flags.instance_private, "component_ref").referred())
            assert_false(valuer.get(Of dispenser(Of Int32, const_pair(Of Byte, Byte))) _
                                   (s, binding_flags.instance_private, "dispenser").binding())
            Return True
        End Function
    End Class
End Class
