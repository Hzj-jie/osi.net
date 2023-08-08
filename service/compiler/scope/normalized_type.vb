
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    ' A helper to always de-alias and apply namespace.
    Public NotInheritable Class normalized_type
        ' TODO: Make these functions as functors for builders.parameter_type.map.
        Public Shared Function logic_type_of(ByVal i As builders.parameter_type) As String
            assert(Not i Is Nothing)
            Return logic_type_of(i.full_type())
        End Function

        Public Shared Function logic_type_of(ByVal type As String) As String
            assert(Not type.null_or_whitespace())
            If current().features().with_namespace() Then
                assert(type.StartsWith(current_namespace_t.namespace_separator))
                Return type.Substring(current_namespace_t.namespace_separator.Length())
            End If
            assert(Not type.StartsWith(current_namespace_t.namespace_separator))
            Return type
        End Function

        Public Shared Function [of](ByVal i As builders.parameter_type) As builders.parameter_type
            assert(Not i Is Nothing)
            Return i.map_type(Function(ByVal s As String) As String
                                  Return current().type_alias()(current_namespace_t.of(s))
                              End Function)
        End Function

        Public Shared Function [of](ByVal type As String) As builders.parameter_type
            Return [of](New builders.parameter_type(type))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
