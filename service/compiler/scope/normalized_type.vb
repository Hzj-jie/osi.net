﻿
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
        Public Shared Function [of](ByVal type As String) As builders.parameter_type
            Return New builders.parameter_type(type).map_type(AddressOf map_type)
        End Function

        Public Shared Function map_type(ByVal i As String) As String
            Return current_namespace_t.of(current().type_alias()(i))
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class normalized_parameter
        Public Shared Function [of](ByVal type As String, ByVal name As String) As builders.parameter
            Return New builders.parameter(type, name).
                                map_type(AddressOf normalized_type.map_type).
                                map_name(Function(ByVal i As String) As String
                                             Return current_namespace_t.of(i)
                                         End Function)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
