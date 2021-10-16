
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly d As defines_t
        Private ReadOnly ta As New type_alias_t()

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            d = New defines_t()
        End Sub

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

        Public NotInheritable Class defines_t
            Private ReadOnly d As New unordered_set(Of String)()

            Public Sub define(ByVal s As String)
                assert(d.emplace(s).second())
            End Sub

            Public Function is_defined(ByVal s As String) As Boolean
                Return d.find(s) <> d.end()
            End Function
        End Class

        Public Function defines() As defines_t
            If is_root() Then
                assert(Not d Is Nothing)
                Return d
            End If
            assert(d Is Nothing)
            Return (+root).d
        End Function

        Public Function type_alias() As type_alias_proxy
            Return New type_alias_proxy(Me)
        End Function
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Public Sub New()
            MyBase.New(bstyle.scope.current())
        End Sub
    End Class
End Class
