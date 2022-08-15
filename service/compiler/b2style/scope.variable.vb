
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        ' TODO: Merge with scope/scope.variable_t.
        Private NotInheritable Shadows Class variable_t
            ' name -> type
            Private ReadOnly m As New unordered_map(Of String, String)()

            Public Function define(ByVal n As typed_node) As Boolean
                assert(Not n Is Nothing)
                assert(n.child_count() >= 2)
                If m.emplace(scope.current_namespace_t.of(n.child(1).input_without_ignored()),
                             scope.current_namespace_t.of(n.child(0).input_without_ignored())).second() Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Variable ",
                            scope.current_namespace_t.of(n.child(1).input_without_ignored()),
                            " has been defined already as ",
                            scope.current_namespace_t.of(n.child(0).input_without_ignored()))
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                assert(Not name.null_or_whitespace())
                Return m.find(scope.current_namespace_t.of(name), type)
            End Function
        End Class

        Public Shadows Structure variable_proxy
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
                name = name.Trim()
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

        Public Shadows Function variables() As variable_proxy
            Return New variable_proxy(Me)
        End Function
    End Class
End Class
