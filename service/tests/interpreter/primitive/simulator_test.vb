
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public NotInheritable Class simulator_test
        Inherits [case]

        Private Shared ReadOnly cases() As pair(Of Byte(), String) = {
            make_case(bool_bytes(True), sim1.as_text()),
            make_case(int32_bytes(100), sim2.as_text())}

        Private Shared Function make_case(ByVal b() As Byte, ByVal str As String) As pair(Of Byte(), String)
            Return pair.emplace_of(b, str)
        End Function

        Private Shared Function execute(ByVal s As String, Optional ByRef sim As simulator = Nothing) As Boolean
            If sim Is Nothing Then
                sim = New simulator()
            End If
            assertion.is_true(sim.import(s))
            sim.execute()
            assertion.is_false(sim.halt())
            assertion.is_true(sim.errors().empty())
            Return True
        End Function

        Private Shared Function case1() As Boolean
            For i As UInt32 = 0 To array_size(cases) - uint32_1
                Dim sim As simulator = Nothing
                If Not execute(cases(CInt(i)).second, sim) Then
                    Return False
                End If
                Dim p As ref(Of Byte()) = sim.access(data_ref.abs(0))
                assertion.array_equal(+p, cases(CInt(i)).first)
            Next
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim sim As simulator = Nothing
            If Not execute(sim5.as_text(), sim) Then
                Return False
            End If
            assertion.equal(sim.access_as_bool(data_ref.abs(0)), False)
            assertion.equal(sim.access_as_uint32(data_ref.abs(1)), CUInt(1326))
            assertion.equal(sim.access_as_uint32(data_ref.abs(2)), CUInt(51))
            assertion.equal(sim.access_as_uint32(data_ref.abs(3)), CUInt(1))
            assertion.equal(sim.access_as_uint32(data_ref.abs(4)), CUInt(50))
            Return True
        End Function

        Private Shared Function access_heap_case() As Boolean
            Dim sim As simulator = Nothing
            If Not execute(access_heap.as_text(), sim) Then
                Return False
            End If

            assertion.equal(sim.access_as_uint32(data_ref.abs(0)), CUInt(1))
            assertion.equal(sim.access_as_uint32(data_ref.abs(1)), CUInt(104))
            assertion.equal(sim.access_ref_as_uint32(sim.access_heap(sim.access_as_uint64(data_ref.abs(2)) - CULng(4))),
                            CUInt(100))
            assertion.equal(sim.access_ref_as_uint32(sim.access_heap(sim.access_as_uint64(data_ref.abs(2)) - CULng(3))),
                            CUInt(101))
            assertion.equal(sim.access_ref_as_uint32(sim.access_heap(sim.access_as_uint64(data_ref.abs(2)) - CULng(2))),
                            CUInt(102))
            assertion.equal(sim.access_ref_as_uint32(sim.access_heap(sim.access_as_uint64(data_ref.abs(2)) - CULng(1))),
                            CUInt(103))
            assertion.equal(sim.access_ref_as_uint32(sim.access_heap(sim.access_as_uint64(data_ref.abs(2)))),
                            CUInt(104))
            Return True
        End Function

        Private Shared Function dealloc_case() As Boolean
            Dim sim As simulator = Nothing
            If Not execute(dealloc.as_text(), sim) Then
                Return False
            End If

            assertion.equal(sim.access_as_uint32(data_ref.abs(0)), CUInt(100))
            assertion.array_equal(assertion.catch_thrown(Of executor_stop_error) _
                                                        (Sub()
                                                             sim.access(data_ref.habs(1))
                                                         End Sub).error_types,
                                  {executor.error_type.heap_access_out_of_boundary})
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2() AndAlso
                   access_heap_case() AndAlso
                   dealloc_case()
        End Function
    End Class
End Namespace
