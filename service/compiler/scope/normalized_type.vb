
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
        Public Shared Function logic_type_of(ByVal type As String) As String
            Return New builders.parameter_type(type).map_type(AddressOf map_type).logic_type()
        End Function

        Public Shared Function map_type(ByVal i As String) As String
            Return current().type_alias()(current_namespace_t.of(i))
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class normalized_parameter
        Public Shared Function [of](ByVal type As String, ByVal name As String) As builders.parameter
            Return [of](New builders.parameter(type, name))
        End Function

        Public Shared Function [of](ByVal i As builders.parameter) As builders.parameter
            assert(Not i Is Nothing)
            Return i.map_type(AddressOf normalized_type.map_type).
                     map_name(Function(ByVal x As String) As String
                                  Return current_namespace_t.of(x)
                              End Function)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
