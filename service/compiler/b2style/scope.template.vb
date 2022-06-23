
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class template_t
            Private ReadOnly m As New unordered_map(Of name_with_namespace, definition)()

            Private NotInheritable Class definition
                Private ReadOnly template As template_template
                Private ReadOnly injected_types As New unordered_set(Of vector(Of String))()

                Public Sub New(ByVal t As template_template)
                    assert(Not t Is Nothing)
                    template = t
                End Sub

                Public Function apply(ByVal types As vector(Of String), ByRef o As String) As Boolean
                    ' TODO: Should resolve type-aliases.
                    If injected_types.emplace(types).second() Then
                        Return template.apply(types, o)
                    End If
                    ' Injected already.
                    o = Nothing
                    Return True
                End Function

                Public Function extended_type_name(ByVal types As vector(Of String)) As String
                    Return template.extended_type_name(types)
                End Function
            End Class

            Public Function define(ByVal name As String,
                                   ByVal type_param_list As vector(Of String),
                                   ByVal body As typed_node,
                                   ByVal name_node As typed_node) As Boolean
                assert(Not name.null_or_whitespace())
                assert(Not body Is Nothing)
                Dim t As template_template = Nothing
                If Not template_template.of(type_param_list, body, name_node, t) Then
                    Return False
                End If
                assert(Not t Is Nothing)
                Dim d As New definition(t)
                If m.emplace(name_with_namespace.of(name), d).second() Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Template [",
                            name_with_namespace.of(name),
                            "] has been defined already.")
                Return False
            End Function

            Public Function resolve(ByVal input_name As String,
                                    ByVal types As vector(Of String),
                                    ByRef extended_type_name As String) As ternary
                assert(Not types.null_or_empty())
                Dim name As name_with_namespace = name_with_namespace.of(input_name)
                Dim d As definition = Nothing
                If Not m.find(name, d) Then
                    Return ternary.unknown
                End If
                assert(Not d Is Nothing)
                Dim s As String = Nothing
                If Not d.apply(types, s) Then
                    Return ternary.false
                End If
                assert(s Is Nothing OrElse Not s.empty_or_whitespace())
                If Not s Is Nothing Then
                    Using If(name.namespace().empty_or_whitespace(),
                             empty_idisposable.instance,
                             scope.current().current_namespace().define(name.namespace()))
                        If Not parser.instance(s, scope.current().root_type_injector().current()) Then
                            Return ternary.false
                        End If
                    End Using
                End If
                ' TODO: Should return b2style name with namespace rather than bstyle.
                extended_type_name = _namespace.bstyle_format.with_namespace(
                                         name.namespace(),
                                         d.extended_type_name(types))
                Return ternary.true
            End Function
        End Class

        Public Structure template_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String,
                                   ByVal type_param_list As vector(Of String),
                                   ByVal body As typed_node,
                                   ByVal name_node As typed_node) As Boolean
                assert(Not body Is Nothing)
                Return s.t.define(name, type_param_list, body, name_node)
            End Function

            Public Function resolve(ByVal n As typed_node, ByRef extended_type_name As String) As Boolean
                assert(Not n Is Nothing)
                assert(n.child_count() = 4)
                Dim types As vector(Of String) = code_gens().of_all_children(n.child(2)).dump()
                Dim name As String = b2style.template.name_of(n)
                Dim s As scope = Me.s
                While Not s Is Nothing
                    Dim r As ternary = s.t.resolve(name, types, extended_type_name)
                    If Not r.unknown_() Then
                        Return r
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user,
                            "Template [",
                            name,
                            "] has not been defined for ",
                            n.input())
                Return False
            End Function
        End Structure

        Public Function template() As template_proxy
            Return New template_proxy(Me)
        End Function
    End Class
End Class
