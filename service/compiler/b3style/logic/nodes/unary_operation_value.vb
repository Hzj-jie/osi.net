
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class unary_operation_value
        Implements code_gen(Of logic_writer)

        Private ReadOnly operator_index As UInt32
        Private ReadOnly suffix As String

        Public Sub New(ByVal operator_index As UInt32, ByVal suffix As String)
            assert(Not suffix.null_or_whitespace())
            Me.operator_index = operator_index
            Me.suffix = suffix
        End Sub

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Dim function_name As String =
                    binary_operation_value.operation_function_name(n.child(operator_index).type_name + suffix)
            Return value_list.build(Sub(ByVal a As Action(Of typed_node))
                                        assert(Not a Is Nothing)
                                        a(n.child(uint32_1 - operator_index))
                                    End Sub,
                                    o) AndAlso
                   function_call.build(function_name, o)
        End Function
    End Class
End Class
