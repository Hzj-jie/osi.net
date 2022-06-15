
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
                    If Not injected_types.emplace(types).second() Then
                        ' Injected already.
                        o = Nothing
                        Return True
                    End If
                    Return template.apply(types, o)
                End Function

                Public Function extended_type_name(ByVal types As vector(Of String)) As String
                    Return template.extended_type_name(types)
                End Function
            End Class

            Public Function define(ByVal type_param_list As vector(Of String), ByVal n As typed_node) As Boolean
                assert(Not n Is Nothing)
                Dim t As template_template = Nothing
                If Not template_template.of(type_param_list, n, t) Then
                    Return False
                End If
                assert(Not t Is Nothing)
                Dim d As New definition(t)
                If Not m.emplace(name_with_namespace.of(t.name()), d).second() Then
                    raise_error(error_type.user,
                                "Template [",
                                name_with_namespace.of(t.name()),
                                "] has been defined already.")
                    Return False
                End If
                Return True
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

            Public Function define(ByVal n As typed_node) As Boolean
                assert(Not n Is Nothing)
                assert(n.child_count() = 2)
                assert(n.child(0).child_count() = 4)
                Dim type_param_list As vector(Of String) = code_gens().of_all_children(n.child(0).child(2)).dump()
                Return s.t.define(type_param_list, n)
            End Function

            Public Function resolve(ByVal name_override As _do(Of String, Boolean),
                                    ByVal n As typed_node,
                                    ByRef extended_type_name As String) As Boolean
                assert(Not name_override Is Nothing)
                assert(Not n Is Nothing)
                assert(n.child_count() = 4)
                Dim types As vector(Of String) = code_gens().of_all_children(n.child(2)).dump()
                Dim name As String = Nothing
                If name_override(name) Then
                    name = template_template.template_name(name, n.child(2).child_count())
                Else
                    name = template_template.template_name(n.child(0), n.child(2).child_count())
                End If
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

            Public Function resolve(ByVal n As typed_node, ByRef extended_type_name As String) As Boolean
                Return resolve(Function(ByRef o As String) As Boolean
                                   Return False
                               End Function,
                               n,
                               extended_type_name)
            End Function
        End Structure

        Public Function template() As template_proxy
            Return New template_proxy(Me)
        End Function
    End Class
End Class
