
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    ' VisibleForTesting
    Public NotInheritable Class template_template
        Inherits scope.template_template(Of target_type_name)

        Public Structure target_type_name
            Implements func_t(Of String)

            Public Function run() As String Implements func_t(Of String).run
                Return "raw-type-name"
            End Function
        End Structure

        Private Sub New(ByVal body As typed_node, ByVal name_node As typed_node, ByVal types As vector(Of String))
            MyBase.New(body, name_node, types)
        End Sub

        ' @VisibleForTesting
        ' TODO: Remove this function.
        Public Shared Function [of](ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef o As template_template) As Boolean
            Return template.build(l, n, o)
        End Function

        Public Shared Function [of](ByVal type_param_list As vector(Of String),
                                    ByVal body As typed_node,
                                    ByVal name_node As typed_node,
                                    ByRef o As template_template) As Boolean
            assert(Not type_param_list.null_or_empty())
            assert(Not body Is Nothing)
            assert(Not name_node Is Nothing)
            assert(body.type_name.Equals("template-body"))
            assert(body.child_count() = 1)
            If type_param_list.size() >
               type_param_list.stream().collect_by(stream(Of String).collectors.unique()).size() Then
                raise_error(error_type.user,
                            "Template ",
                            name_node.input_without_ignored(),
                            " has duplicated template type parameters: [",
                            type_param_list.str(", "),
                            "]")
                Return False
            End If
            o = New template_template(body.child(), name_node, type_param_list)
            Return True
        End Function
    End Class
End Class
