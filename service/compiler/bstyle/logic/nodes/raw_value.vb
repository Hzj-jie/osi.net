
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private MustInherit Class raw_value
        Implements code_gen(Of logic_writer)

        Private ReadOnly code_type As String

        Protected Sub New(ByVal code_type As String)
            assert(Not code_type.null_or_whitespace())
            Me.code_type = code_type
        End Sub

        Protected MustOverride Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean

        Protected Shared Function build(ByVal i As data_block,
                                        ByVal code_type As String,
                                        ByVal o As logic_writer) As Boolean
            assert(Not i Is Nothing)
            assert(Not code_type.null_or_whitespace())
            assert(Not o Is Nothing)
            Return compiler.logic.builders.of_copy_const(
                        scope.current().value_target().with_temp_target(
                            scope.current_namespace_t.if_supported.in_global_namespace(code_type),
                            o).only(), i).to(o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim d As data_block = Nothing
            If Not parse(n, d) Then
                raise_error(error_type.user, "Cannot parse data to ", code_type, " ", n.trace_back_str())
                Return False
            End If
            Return build(d, code_type, o)
        End Function
    End Class
End Class
