
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class delegate_t
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
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal return_type As String,
                                   ByVal name As String,
                                   ByVal parameters() As builders.parameter_type) As Boolean
                Return s.de.define(name,
                                   New function_signature(
                                           name,
                                           s.type_alias()(return_type),
                                           streams.of(parameters).
                                                   map(AddressOf s.type_alias().canonical_of).
                                                   to_array()))
            End Function

            Public Function retrieve(ByVal name As String, ByRef o As function_signature) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.de.retrieve(name, o) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function
        End Structure

        Public Function delegates() As delegate_proxy
            Return New delegate_proxy(Me)
        End Function
    End Class
End Class
