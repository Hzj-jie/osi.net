
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    ' A helper to always de-alias and apply namespace.
    Public NotInheritable Class normalized_type
        Public Shared Function logic_type_of(ByVal type As String) As String
            Return remove_namespace_prefix([of](type).logic_type())
        End Function

        Public Shared Function [of](ByVal type As String) As builders.parameter_type
            Return New builders.parameter_type(type).map_type(AddressOf map_type)
        End Function

        Public Shared Function map_type(ByVal i As String) As String
            Return current().type_alias()(current_namespace_t.of(i))
        End Function

        Public Shared Function remove_namespace_prefix(ByVal s As String) As String
            assert(s.StartsWith(current_namespace_t.namespace_separator))
            Return s.Substring(current_namespace_t.namespace_separator.Length())
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class normalized_parameter
        Public Shared Function logic_name_type_of(ByVal i As builders.parameter) As pair(Of String, String)
            assert(Not i Is Nothing)
            Return pair.emplace_of(normalized_type.remove_namespace_prefix(current_namespace_t.of(i.name)),
                                   normalized_type.remove_namespace_prefix(normalized_type.map_type(i.type)))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
