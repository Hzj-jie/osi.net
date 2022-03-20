
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    ' TODO: Avoid using constructors directly.
    Public NotInheritable Class caller_test
        Inherits executor_case

        Public Sub New()
            MyBase.New(New _callee("add",
                                   "type*",
                                   New paragraph(
                                       New _define("result", scope.type_t.variable_type),
                                       New _add("result", "parameter1", "parameter2"),
                                       New _return("add", "result")
                                   ),
                                   pair.emplace_of("parameter1", scope.type_t.variable_type),
                                   pair.emplace_of("parameter2", scope.type_t.variable_type),
                                   pair.emplace_of("parameter3", scope.type_t.variable_type)),
                       New _define("parameter1", scope.type_t.variable_type),
                       New _define("parameter2", scope.type_t.variable_type),
                       New _define("parameter3", scope.type_t.variable_type),
                       New _define("result", scope.type_t.variable_type),
                       New _copy_const("parameter1", New data_block(100)),
                       New _copy_const("parameter2", New data_block(200)),
                       New _copy_const("parameter3", New data_block(10000)),
                       New _caller("add", "result", "parameter1", "parameter2", "parameter3"))
        End Sub

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            If assertion.equal(e.get().stack_size(), CULng(4)) Then
                assertion.equal(e.get().access_as_uint32(data_ref.abs(0)), CUInt(100))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(1)), CUInt(200))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(2)), CUInt(10000))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(3)), CUInt(300))
            End If
        End Sub
    End Class
End Namespace
