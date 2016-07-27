
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
            Return emplace_make_pair(b, str)
        End Function

        Public Overrides Function run() As Boolean
            For i As UInt32 = 0 To array_size(cases) - uint32_1
                Dim sim As simulator = Nothing
                sim = New simulator()
                assert_true(sim.import(cases(i).second))
                sim.execute()
                assert_false(sim.halt())
                assert_true(sim.errors().empty())
                Dim p As pointer(Of Byte()) = Nothing
                p = sim.access_stack(data_ref.abs(0))
                assert_array_equal(+p, cases(i).first)
            Next
            Return True
        End Function
    End Class
End Namespace
