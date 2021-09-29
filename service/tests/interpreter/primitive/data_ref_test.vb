
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.interpreter.primitive

Namespace primitive
    Public NotInheritable Class data_ref_exportable_test
        Inherits exportable_test(Of data_ref)

        Protected Overrides Function create() As data_ref
            Dim i As Int64 = 0
            Dim r As data_ref = data_ref.random(i)
            assertion.is_true((r.offset() < 0) = (i < 0))
            assertion.equal(r.export(), i)
            If r.relative() Then
                assertion.equal(data_ref.rel(r.offset()), r)
            ElseIf r.absolute() Then
                assertion.equal(data_ref.abs(r.offset()), r)
            ElseIf r.heap() Then
                assertion.equal(data_ref.heap(r.offset()), r)
            Else
                assert(False)
            End If
            Return r
        End Function
    End Class

    <test>
    Public NotInheritable Class data_ref_exportable_test2
        Private NotInheritable Class test_case
            Public ReadOnly str As String
            Public ReadOnly offset As Int64
            Public ReadOnly absolute As Boolean
            Public ReadOnly relative As Boolean
            Public ReadOnly heap As Boolean

            Private Sub New(ByVal str As String,
                            ByVal offset As Int64,
                            ByVal absolute As Boolean,
                            ByVal relative As Boolean,
                            ByVal heap As Boolean)
                Me.str = str
                Me.offset = offset
                Me.absolute = absolute
                Me.relative = relative
                Me.heap = heap
            End Sub

            Public Shared Function of_abs(ByVal offset As Int64) As test_case
                Return New test_case(strcat("abs", offset), offset, True, False, False)
            End Function

            Public Shared Function of_rel(ByVal offset As Int64) As test_case
                Return New test_case(strcat("rel", offset), offset, False, True, False)
            End Function

            Public Shared Function of_heap(ByVal offset As Int64) As test_case
                Return New test_case(strcat("heap", offset), offset, False, False, True)
            End Function
        End Class

        Private Shared ReadOnly tests As vector(Of test_case) = vector.of(
            test_case.of_abs(0),
            test_case.of_abs(data_ref.max_value),
            test_case.of_abs(data_ref.abs_min_value),
            test_case.of_rel(0),
            test_case.of_rel(data_ref.max_value),
            test_case.of_rel(data_ref.rel_min_value),
            test_case.of_heap(0),
            test_case.of_heap(data_ref.max_value),
            test_case.of_heap(data_ref.heap_min_value))

        <test>
        Private Shared Sub run()
            Dim r As New data_ref()
            For i As UInt32 = 0 To tests.size() - uint32_1
                assertion.is_true(r.import(tests(i).str))
                assertion.equal(r.offset(), tests(i).offset)
                assertion.equal(tests(i).absolute, r.absolute())
                assertion.equal(tests(i).relative, r.relative())
                assertion.equal(tests(i).heap, r.heap())
            Next
        End Sub
    End Class
End Namespace
