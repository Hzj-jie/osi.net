
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class caller_test
        Inherits executor_case

        Private Shared ReadOnly anchors As anchors

        Shared Sub New()
            anchors = New anchors()
        End Sub

        Public Sub New()
            MyBase.New(New callee(anchors,
                                  types.empty,
                                  "add",
                                  "type*",
                                  unique_ptr.[New](New paragraph(
                                      New define(anchors, types.empty, "result", types.variable_type),
                                      New add(types.empty, "result", "parameter1", "parameter2"),
                                      New [return](anchors, types.empty, "add", "result")
                                  )),
                                  pair.emplace_of("parameter1", types.variable_type),
                                  pair.emplace_of("parameter2", types.variable_type),
                                  pair.emplace_of("parameter3", types.variable_type)),
                       New define(anchors, types.empty, "parameter1", types.variable_type),
                       New define(anchors, types.empty, "parameter2", types.variable_type),
                       New define(anchors, types.empty, "parameter3", types.variable_type),
                       New define(anchors, types.empty, "result", types.variable_type),
                       New copy_const(types.empty, "parameter1", unique_ptr.[New](New data_block(100))),
                       New copy_const(types.empty, "parameter2", unique_ptr.[New](New data_block(200))),
                       New copy_const(types.empty, "parameter3", unique_ptr.[New](New data_block(10000))),
                       New caller(anchors, types.empty, "add", "result", "parameter1", "parameter2", "parameter3"))
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
            If assertion.equal(e.get().stack_size(), CULng(4)) Then
                assertion.equal(e.get().access_as_uint32(data_ref.abs(0)), CUInt(100))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(1)), CUInt(200))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(2)), CUInt(10000))
                assertion.equal(e.get().access_as_uint32(data_ref.abs(3)), CUInt(300))
            End If
        End Sub
    End Class
End Namespace
