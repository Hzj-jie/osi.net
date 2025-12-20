
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Private NotInheritable Class _function
        Implements scope.template_t.name_node, scope.template_t.name

        Private Function name_node_of(ByVal n As typed_node, ByRef o As typed_node) As Boolean _
                                     Implements scope.template_t.name_node.of
            assert(Not n Is Nothing)
            o = n.child(1)
            Return True
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean _
                                Implements scope.template_t.name.of
            o = template_name_of(scope.function_name.of(scope.template_t.name_node_of(n)),
                                 scope.template_t.type_param_count(n),
                                 param_types(scope.template_t.body_of(n)))
            Return True
        End Function

        Public Shared Function template_name_of(ByVal function_name As String,
                                                ByVal type_param_count As UInt32,
                                                ByVal param_types As vector(Of String)) As String
            assert(Not function_name.null_or_whitespace())
            assert(type_param_count > 0)
            assert(Not param_types Is Nothing)
            Dim r As New StringBuilder(scope.template_t.name_of(function_name, type_param_count))
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
