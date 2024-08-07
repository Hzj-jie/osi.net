
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class while_then_test
        Inherits executor_case

        Public Sub New()
            MyBase.New(
                New _define("state", scope.type_t.variable_type),
                New _define("result", scope.type_t.variable_type),
                New _define("i", scope.type_t.variable_type),
                New _define("1", scope.type_t.variable_type),
                New _define("50", scope.type_t.variable_type),
                New _copy_const("state", New data_block(True)),
                New _copy_const("1", New data_block(1)),
                New _copy_const("50", New data_block(50)),
                New _while_then("state", New paragraph(
                    New _add("i", "i", "1"),
                    New _add("result", "result", "i"),
                    New _less_or_equal("state", "i", "50")
                ))
            )
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            assertion.equal(e.get().access_as_bool(data_ref.abs(0)), False)
            assertion.equal(e.get().access_as_uint32(data_ref.abs(1)), CUInt(1326))
            assertion.equal(e.get().access_as_uint32(data_ref.abs(2)), CUInt(51))
            assertion.equal(e.get().access_as_uint32(data_ref.abs(3)), CUInt(1))
            assertion.equal(e.get().access_as_uint32(data_ref.abs(4)), CUInt(50))
        End Sub
    End Class
End Namespace
