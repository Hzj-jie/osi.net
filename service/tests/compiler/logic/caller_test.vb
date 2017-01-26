
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class caller_test
        Inherits executor_case

        Private Shared ReadOnly anchors As anchors

        Shared Sub New()
            anchors = New anchors()
        End Sub

        Public Sub New()
            MyBase.New(New callee(anchors,
                                  "add",
                                  unique_ptr.[New](New paragraph(
                                      New define("result", types.variable_type),
                                      New add(types.empty, "result", "parameter1", "parameter2"),
                                      New [return](anchors, "add", "result")
                                  )),
                                  emplace_make_pair("parameter1", types.variable_type),
                                  emplace_make_pair("parameter2", types.variable_type)),
                       New define("parameter1", types.variable_type),
                       New define("parameter2", types.variable_type),
                       New define("result", types.variable_type),
                       New copy_const(types.empty, "parameter1", unique_ptr.[New](New data_block(100))),
                       New copy_const(types.empty, "parameter2", unique_ptr.[New](New data_block(200))),
                       New caller(anchors, "add", "result", "parameter1", "parameter2"))
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                anchors.clear()
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overrides Sub check_result(ByVal e As not_null(Of simulator))
            If assert_equal(e.get().stack_size(), CULng(3)) Then
                assert_equal(e.get().access_stack_as_uint32(data_ref.abs(0)), CUInt(100))
                assert_equal(e.get().access_stack_as_uint32(data_ref.abs(1)), CUInt(200))
                assert_equal(e.get().access_stack_as_uint32(data_ref.abs(2)), CUInt(300))
            End If
        End Sub
    End Class
End Namespace
