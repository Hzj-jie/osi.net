
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class incoming_test
        Inherits [case]

        Private ReadOnly c As collection
        Private ReadOnly p As parameter
        Private component As ref_instance(Of component)
        Private dispenser As dispenser(Of Int32, const_pair(Of UInt16, UInt16))

        Public Sub New()
            c = New collection()
            p = New parameter(1000)
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                assert_true(c.[New](p, component, dispenser))
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim s As vector(Of shared_component(Of UInt16, UInt16, component, Int32, parameter)) = Nothing
            s = _new(s)
            AddHandler c.new_shared_component_exported,
                       Sub(ByVal new_component As shared_component(Of UInt16, UInt16, component, Int32, parameter))
                           s.emplace_back(new_component)
                       End Sub
            Dim p As pointer(Of Int32) = Nothing
            p = New pointer(Of Int32)()
            If assert_true(component.referred()) Then
                For i As UInt16 = 0 To 10
                    For j As UInt16 = 1 To 10
                        assert(shared_component_test.component.is_valid_port(j))
                        component.get().push(200, i, j)
                        If assert_equal(s.size(), CUInt(i) * 9 + j) Then
                            assert_true(async_sync(s(i * 9 + j).receiver.receive(p)))
                            assert_equal(+p, 200)

                            For k As Int32 = 0 To 1000
                                component.get().push(k, i, j)
                                assert_true(async_sync(s(i * 9 + j).receiver.receive(p)))
                                assert_equal(+p, k)
                            Next
                        End If

                        If i > 0 Then
                            For k As UInt16 = 0 To i - uint32_1
                                For l As UInt16 = 1 To 10
                                    component.get().push(k * l, k, l)
                                    assert_true(async_sync(s(k * 9 + l).receiver.receive(p)))
                                    assert_equal(+p, k * l)
                                Next
                            Next
                        End If
                        If j > 1 Then
                            For k As UInt16 = 1 To j - uint32_1
                                component.get().push(k * i, i, k)
                                assert_true(async_sync(s(i * 9 + k).receiver.receive(p)))
                                assert_equal(+p, k * i)
                            Next
                        End If
                    Next
                Next
            End If
            If assert_more(s.size(), uint32_0) Then
                For i As UInt32 = 0 To s.size() - uint32_1
                    s(i).dispose()
                Next
            End If
            assert_equal(dispenser.binding_count(), uint32_1)
            assert_equal(component.ref_count(), uint32_1)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_true(dispenser.release())
            assert_equal(dispenser.binding_count(), uint32_0)
            assert_true(dispenser.expired())
            dispenser.wait_for_stop()
            assert_true(dispenser.stopped())
            assert_equal(component.ref_count(), uint32_0)
            assert_false(component.referred())
            Return MyBase.finish()
        End Function
    End Class
End Class
