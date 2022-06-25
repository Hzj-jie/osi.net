
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class _function
        Implements code_gen(Of typed_node_writer), template.name_node, template.name

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim function_name As String = _namespace.bstyle_format.of(n.child(1))
            Using scope.current().start_scope().current_function().define(function_name)
                Dim fo As New typed_node_writer()
                Return code_gens().of_all_children(n).build(fo) AndAlso
                       o.append(scope.current().call_hierarchy().filter(function_name, AddressOf fo.str))
            End Using
        End Function

        Private Function name_node_of(ByVal n As typed_node) As typed_node Implements template.name_node.of
            assert(Not n Is Nothing)
            Return n.child(1)
        End Function

        Private Function name_of(ByVal n As typed_node) As String Implements template.name.of
            Return template_name_of(template.name_node_of(n).input_without_ignored(),
                                    template.type_param_count(n),
                                    New vector(Of String)())
        End Function

        Public Shared Function template_name_of(ByVal function_name As String,
                                                ByVal type_param_count As UInt32,
                                                ByVal param_types As vector(Of String)) As String
            assert(Not function_name.null_or_whitespace())
            assert(type_param_count > 0)
            assert(Not param_types Is Nothing)
            Dim r As New StringBuilder(template.name_of(function_name, type_param_count))
            Dim i As UInt32 = 0
            While i < param_types.size()
                r.Append("&").
                  Append(logic.builders.parameter_type.remove_ref(param_types(i)))
                i += uint32_1
            End While
            Return r.ToString()
        End Function
    End Class
End Class
