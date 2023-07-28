
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
        Public Shared Function full_type_of(ByVal type As String) As String
            ' TODO: Add a prefix for logic_type to avoid being called twice on one type.
            assert(Not type.null_or_whitespace())
            ' assert(Not type.StartsWith("@"))
            Dim s As String = [of](type).full_type()
            If current().features().with_namespace() Then
                assert(s.StartsWith(current_namespace_t.namespace_separator))
                s = s.Substring(current_namespace_t.namespace_separator.Length())
            End If
            ' Return "@" + s
            Return s
        End Function

        Public Shared Function full_type_of(ByVal i As builders.parameter_type) As String
            assert(Not i Is Nothing)
            Return full_type_of(i.full_type())
        End Function

        Public Shared Function [of](ByVal type As String) As builders.parameter_type
            Return New builders.parameter_type(type).map_type(
                        Function(ByVal i As String) As String
                            Return current().type_alias()(current_namespace_t.of(i))
                        End Function)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
