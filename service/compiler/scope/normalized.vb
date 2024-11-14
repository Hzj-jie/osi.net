
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class type_name
        Public Shared Function [of](ByVal type As typed_node) As String
            assert(Not type Is Nothing)
            ' May use word().str()
            Return type.input_without_ignored()
        End Function

        Private Sub New()
        End Sub
    End Class

    ' A helper to always de-alias and apply namespace.
    Public NotInheritable Class normalized_type
        ' Calling these functions twice shouldn't make a difference.
        Public Shared Function parameter_type_of(ByVal type As String) As builders.parameter_type
            Return builders.parameter_type.of(type).map_type(AddressOf [of])
        End Function

        Public Shared Function parameter_type_of(ByVal type As typed_node) As builders.parameter_type
            Return parameter_type_of(type_name.of(type))
        End Function

        Public Shared Function [of](ByVal type As String) As String
            Return current().type_alias()(current_namespace_t.of(type))
        End Function

        Public Shared Function [of](ByVal type As typed_node) As String
            Return [of](type_name.of(type))
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class variable_name
        Public Shared Function [of](ByVal name As typed_node) As String
            assert(Not name Is Nothing)
            ' May use word().str()
            Return name.input_without_ignored()
        End Function

        Private Sub New()
        End Sub
    End Class

    ' A helper to apply namespace.
    Public NotInheritable Class fully_qualified_variable_name
        ' Calling these functions twice shouldn't make a difference.
        Public Shared Function [of](ByVal name As String) As String
            assert(Not name.null_or_whitespace())
            ' Not in a function, treat the variable namespace-qualified.
            If scope(Of T).current().is_root() Then
                Return current_namespace_t.of(name)
            End If
            Return name
        End Function

        Public Shared Function [of](ByVal name As typed_node) As String
            Return [of](variable_name.of(name))
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class function_name
        Public Shared Function [of](ByVal name As typed_node) As String
            assert(Not name Is Nothing)
            ' May use word().str()
            Return name.input_without_ignored()
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class fully_qualified_function_name
        Public Shared Function [of](ByVal name As typed_node) As String
            Return [of](function_name.of(name))
        End Function

        Public Shared Function [of](ByVal name As String) As String
            assert(Not name.null_or_whitespace())
            ' TODO: Avoid the hack of not adding :: for main.
            If name.Equals("main") Then
                Return "main"
            End If
            Return current_namespace_t.of(name)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
