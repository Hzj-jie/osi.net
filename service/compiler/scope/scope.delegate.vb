
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of _ACCESSOR As accessor, T As scope(Of _ACCESSOR, T))
    Public NotInheritable Class delegate_t
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
        Private ReadOnly s As T
        Private ReadOnly getter As Func(Of T, delegate_t)
        Private ReadOnly type_alias As Func(Of T, String, String)

        Public Sub New(ByVal s As T,
                       ByVal getter As Func(Of T, delegate_t),
                       ByVal type_alias As Func(Of T, String, String))
            assert(Not s Is Nothing)
            assert(Not getter Is Nothing)
            assert(Not type_alias Is Nothing)
            Me.s = s
            Me.getter = getter
            Me.type_alias = type_alias
        End Sub

        Public Function define(ByVal return_type As String,
                               ByVal name As String,
                               ByVal parameters() As builders.parameter_type) As Boolean
            assert(Not parameters Is Nothing)
            assert(return_type.Equals(type_alias(s, return_type)))
            For Each parameter As builders.parameter_type In parameters
                assert(Not parameter Is Nothing)
                assert(parameter.type.Equals(type_alias(s, parameter.type)))
            Next
            Return getter(s).define(name, New function_signature(name, return_type, parameters))
        End Function

        Public Function retrieve(ByVal name As String, ByRef o As function_signature) As Boolean
            Dim s As T = Me.s
            While Not s Is Nothing
                If getter(s).retrieve(name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            Return False
        End Function
    End Structure
End Class
