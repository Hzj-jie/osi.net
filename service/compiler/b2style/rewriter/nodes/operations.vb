
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Public NotInheritable Class operations
        Private Const self_prefix As String = "self-"

        Private Shared Function function_name(ByVal type_name As String) As String
            assert(Not type_name.null_or_whitespace())
            Return namespace_.bstyle_format_in_root_namespace("b2style", type_name.Replace("-"c, "_"c))
        End Function

        Public Shared Function pre_function_name(ByVal n As typed_node) As String
            Return strcat(function_name(n), "_pre")
        End Function

        Public Shared Function post_function_name(ByVal n As typed_node) As String
            Return strcat(function_name(n), "_post")
        End Function

        Public Shared Function function_name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            Return function_name(n.type_name)
        End Function

        Public Shared Function self_function_name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            assert(n.child().type_name.StartsWith(self_prefix))
            Return function_name(n.child().type_name.Substring(self_prefix.Length()))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
