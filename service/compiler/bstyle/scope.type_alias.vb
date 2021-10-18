
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class type_alias_t
            Private ReadOnly m As New unordered_map(Of String, String)()

            Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
                If m.emplace([alias], canonical).second OrElse m([alias]).Equals(canonical) Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Alias ",
                            [alias],
                            " of canonical type ",
                            canonical,
                            " has been defined already as ",
                            m([alias]),
                            ".")
                Return False
            End Function

            Default Public ReadOnly Property _D(ByVal [alias] As String) As String
                Get
                    Dim c As String = Nothing
                    While m.find([alias], c)
                        [alias] = c
                        c = Nothing
                    End While
                    Return [alias]
                End Get
            End Property
        End Class

        Public NotInheritable Class type_alias_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal [alias] As String, ByVal canonical As String) As Boolean
                Return current().ta.define([alias], canonical)
            End Function

            Default Public ReadOnly Property _D(ByVal [alias] As String) As String
                Get
                    Dim s As scope = Me.s
                    While Not s Is Nothing
                        [alias] = s.ta([alias])
                        s = s.parent
                    End While
                    Return [alias]
                End Get
            End Property

            Public Function canonical_of(ByVal p As builders.parameter) As builders.parameter
                assert(Not p Is Nothing)
                Return New builders.parameter(Me(p.type), p.name)
            End Function

            Private Sub New()
            End Sub
        End Class

        Public Function type_alias() As type_alias_proxy
            Return New type_alias_proxy(Me)
        End Function
    End Class
End Class
