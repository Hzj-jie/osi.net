
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    ' A helper to always de-alias and apply namespace.
    ' TODO: Should respect b3style._disable_namespace.
    Public NotInheritable Class normalized_type
        Public Shared ReadOnly logic_type_of As Func(Of String, String) =
            Function(ByVal type As String) As String
                assert(Not type.null_or_whitespace())
                If current().features().with_namespace() Then
                    assert(type.StartsWith(current_namespace_t.namespace_separator))
                    Return type.Substring(current_namespace_t.namespace_separator.Length())
                End If
                assert(Not type.StartsWith(current_namespace_t.namespace_separator))
                Return type
            End Function

        Public Shared ReadOnly [of] As Func(Of String, String) =
            Function(ByVal type As String) As String
                Return current().type_alias()(current_namespace_t.of(type))
            End Function

        Private Sub New()
        End Sub
    End Class
End Class
