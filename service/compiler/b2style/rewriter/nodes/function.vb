
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
        Implements code_gen(Of typed_node_writer), scope.template_t.name_node, scope.template_t.name

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

        Private Function name_node_of(ByVal n As typed_node, ByRef o As typed_node) As Boolean _
                                     Implements scope.template_t.name_node.of
            assert(Not n Is Nothing)
            o = n.child(1)
            Return True
        End Function

        Private Shared Function param_types(ByVal n As typed_node) As vector(Of String)
            Return New vector(Of String)()

            ' Following implementation does not work, b2style has no information about the types, the
            ' function_call_with_template cannot create correct parameter types.
#If 0 Then
            assert(Not n Is Nothing)
            If n.child_count() = 5 Then
                Return New vector(Of String)()
            End If
            assert(n.child_count() = 6)
            Return n.child(3).
                     children().
                     map(Function(ByVal node As typed_node) As String
                             assert(Not node Is Nothing)
                             If node.type_name.Equals("param-with-comma") Then
                                 node = node.child(0)
                             End If
                             assert(node.type_name.Equals("param"))
                             Return node.child(0).input_without_ignored()
                         End Function).
                     collect_to(Of vector(Of String))()
#End If
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean _
                                Implements scope.template_t.name.of
            o = template_name_of(scope.template_builder.name_node_of(n).input_without_ignored(),
                                 scope.template_builder.type_param_count(n),
                                 param_types(scope.template_builder.body_of(n)))
            Return True
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
