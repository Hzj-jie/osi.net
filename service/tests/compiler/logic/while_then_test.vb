
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class while_then_test
        Inherits executor_case

        Public Sub New()
            MyBase.New(
                New define("state", types.variable_type),
                New define("result", types.variable_type),
                New define("i", types.variable_type),
                New define("1", types.variable_type),
                New define("50", types.variable_type),
                New copy_const(types.empty, "state", unique_ptr.[New](New data_block(True))),
                New copy_const(types.empty, "1", unique_ptr.[New](New data_block(1))),
                New copy_const(types.empty, "50", unique_ptr.[New](New data_block(50))),
                New while_then("state", unique_ptr.[New](New paragraph(
                    New add(types.empty, "i", "i", "1"),
                    New add(types.empty, "result", "result", "i"),
                    New less_or_equal(types.empty, "state", "i", "50")
                )))
            )
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assert_equal(e.get().access_stack_as_bool(data_ref.abs(0)), False)
            assert_equal(e.get().access_stack_as_uint32(data_ref.abs(1)), CUInt(1326))
            assert_equal(e.get().access_stack_as_uint32(data_ref.abs(2)), CUInt(51))
            assert_equal(e.get().access_stack_as_uint32(data_ref.abs(3)), CUInt(1))
            assert_equal(e.get().access_stack_as_uint32(data_ref.abs(4)), CUInt(50))
        End Sub
    End Class
End Namespace
