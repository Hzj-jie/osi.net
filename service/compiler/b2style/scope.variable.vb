
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class variable_t
            ' type -> name
            Private ReadOnly m As New unordered_map(Of String, String)()

            Public Function define(ByVal n As typed_node) As Boolean
                assert(Not n Is Nothing)
                assert(n.child_count() >= 2)
                If m.emplace(n.child(0).children_word_str(), n.child(1).children_word_str()).second() Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Variable ",
                            n.child(1).children_word_str(),
                            " has been defined already as ",
                            n.child(0).children_word_str())
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                assert(Not name.null_or_whitespace())
                Return m.find(name, type)
            End Function
        End Class

        Public Structure variable_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Shared Function define() As Func(Of typed_node, Boolean)
                Return Function(ByVal n As typed_node) As Boolean
                           Return current().variables().define(n)
                       End Function
            End Function

            Public Function define(ByVal n As typed_node) As Boolean
                Return s.v.define(n)
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                assert(Not name.null_or_whitespace())
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.v.resolve(name, type) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function
        End Structure

        Public Function variables() As variable_proxy
            Return New variable_proxy(Me)
        End Function
    End Class
End Class
