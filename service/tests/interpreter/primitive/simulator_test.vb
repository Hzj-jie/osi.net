﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public Class simulator_test
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
                If Not execute(cases(i).second, sim) Then
                    Return False
                End If
                Dim p As pointer(Of Byte()) = Nothing
                p = sim.access_stack(data_ref.abs(0))
                assertion.array_equal(+p, cases(i).first)
            Next
            Return True
        End Function

        Private Shared Function case2() As Boolean
            Dim sim As simulator = Nothing
            If Not execute(sim5.as_text(), sim) Then
                Return False
            End If
            assertion.equal(sim.access_stack_as_bool(data_ref.abs(0)), False)
            assertion.equal(sim.access_stack_as_uint32(data_ref.abs(1)), CUInt(1326))
            assertion.equal(sim.access_stack_as_uint32(data_ref.abs(2)), CUInt(51))
            assertion.equal(sim.access_stack_as_uint32(data_ref.abs(3)), CUInt(1))
            assertion.equal(sim.access_stack_as_uint32(data_ref.abs(4)), CUInt(50))
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return case1() AndAlso
                   case2()
        End Function
    End Class
End Namespace
