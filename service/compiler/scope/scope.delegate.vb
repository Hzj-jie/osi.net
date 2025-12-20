
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Protected NotInheritable Class delegate_t
        Private ReadOnly s As New unordered_map(Of String, function_signature)()

        Public Function define(ByVal name As String, ByVal signature As function_signature) As Boolean
            assert(Not signature Is Nothing)
            assert(Not name.null_or_whitespace())
            If s.emplace(name, signature).second() Then
                Return True
            End If
            raise_error(error_type.user, "Delegate type ", name, " has been defined already as ", s(name))
            Return False
        End Function

        Public Function retrieve(ByVal name As String, ByRef o As function_signature) As Boolean
            assert(Not name.null_or_whitespace())
            Return s.find(name, o)
        End Function
    End Class

    Public Structure delegate_proxy
        Public Function define(ByVal return_type As String,
                               ByVal name As String,
                               ByVal parameters() As builders.parameter_type) As Boolean
            assert(Not parameters Is Nothing)
            assert(return_type.equal_to(normalized_type.parameter_type_of(return_type).full_type()))
            For Each parameter As builders.parameter_type In parameters
                assert(Not parameter Is Nothing)
                assert(parameter.full_type().equal_to(
                       parameter.map_type(AddressOf normalized_type.of).full_type()))
            Next
            Return scope(Of T).current().
                               myself().
                               delegates().
                               define(name, New function_signature(name, return_type, parameters))
        End Function

        Public Function retrieve(ByVal name As String, ByRef o As function_signature) As Boolean
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                If s.myself().delegates().retrieve(name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            Return False
        End Function
    End Structure
End Class
