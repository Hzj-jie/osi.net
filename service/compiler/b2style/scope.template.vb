
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class template_t
            Private ReadOnly m As New unordered_map(Of name_with_namespace, definition)()
            Private ReadOnly tbr As New template_body_reparser(m)

            Private NotInheritable Class definition
                Public ReadOnly template As template_template
                Public ReadOnly injected_types As New unordered_set(Of vector(Of String))()
                Public ReadOnly injector As New typed_node_writer()

                Public Sub New(ByVal t As template_template)
                    assert(Not t Is Nothing)
                    template = t
                End Sub
            End Class

            Private NotInheritable Class template_body_reparser
                Inherits code_gens(Of typed_node_writer).reparser(Of b2style.parser)

                Private ReadOnly m As unordered_map(Of name_with_namespace, definition)
                Private ReadOnly types As New one_off(Of vector(Of String))()

                Public Sub New(ByVal m As unordered_map(Of name_with_namespace, definition))
                    assert(Not m Is Nothing)
                    Me.m = m
                End Sub

                Public Function with_types(ByVal types As vector(Of String)) As template_body_reparser
                    assert(Me.types.set(types))
                    Return Me
                End Function

                Protected Overrides Function wrapper(ByVal n As typed_node) As IDisposable
                    ' The definition should be searched already in resolve.
                    Dim name As name_with_namespace = template_name(n)
                    Dim d As definition = +m.find_opt(name)
                    assert(Not d Is Nothing)
                    If name.namespace().empty_or_whitespace() Then
                        Return MyBase.wrapper(n)
                    End If
                    Return scope.current().current_namespace().define(name.namespace())
                End Function

                Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
                    ' The definition should be searched already in resolve.
                    Dim d As definition = +m.find_opt(template_name(n))
                    assert(Not d Is Nothing)
                    Dim types As vector(Of String) = Me.types.get()
                    ' TODO: Should resolve type-aliases.
                    If Not d.injected_types.emplace(types).second() Then
                        ' Injected already.
                        s = ""
                        Return True
                    End If
                    Return d.template.apply(types, s)
                End Function
            End Class

            Public Shared Function template_name(ByVal n As typed_node) As name_with_namespace
                assert(Not n Is Nothing)
                assert(n.child_count() >= 4)
                Return _namespace.of_name_with_namespace(
                           template_template.template_name(n.child(0), n.child(2).child_count()))
            End Function

            Private Shared Function template_types(ByVal l As code_gens(Of typed_node_writer),
                                                   ByVal n As typed_node,
                                                   ByRef o As vector(Of String)) As Boolean
                assert(Not l Is Nothing)
                assert(Not n Is Nothing)
                assert(n.child_count() >= 4)
                Return l.of_all_children(n.child(2)).dump(o)
            End Function

            Public Function define(ByVal l As code_gens(Of typed_node_writer),
                                   ByVal n As typed_node,
                                   ByVal o As typed_node_writer) As Boolean
                assert(Not n Is Nothing)
                assert(Not o Is Nothing)
                Dim t As template_template = Nothing
                If Not template_template.of(l, n, t) Then
                    Return False
                End If
                assert(Not t Is Nothing)
                Dim d As New definition(t)
                If Not m.emplace(_namespace.of_name_with_namespace(t.name()), d).second() Then
                    raise_error(error_type.user, "Template [", t.name(), "] has been defined already.")
                    Return False
                End If
                o.append(d.injector)
                Return True
            End Function

            Public Function resolve(ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef extended_type_name As String) As [optional](Of Boolean)
                assert(Not n Is Nothing)
                Dim name As name_with_namespace = template_name(n)
                Dim d As definition = Nothing
                If Not m.find(name, d) Then
                    Return [optional].empty(Of Boolean)()
                End If
                assert(Not d Is Nothing)
                Dim types As vector(Of String) = Nothing
                If Not template_types(l, n, types) Then
                    Return [optional].of(False)
                End If
                If Not tbr.with_types(types).build(n, d.injector) Then
                    Return [optional].of(False)
                End If
                extended_type_name = _namespace.bstyle_format.with_namespace(
                                         name.namespace(),
                                         d.template.extended_type_name(types))
                Return [optional].of(True)
            End Function
        End Class

        Public Structure template_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal l As code_gens(Of typed_node_writer),
                                   ByVal n As typed_node,
                                   ByVal o As typed_node_writer) As Boolean
                Return s.t.define(l, n, o)
            End Function

            Public Function resolve(ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef extended_type_name As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    Dim r As [optional](Of Boolean) = s.t.resolve(l, n, extended_type_name)
                    If r Then
                        Return +r
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user,
                            "Template [",
                            template_t.template_name(n),
                            "] has not been defined.")
                Return False
            End Function
        End Structure

        Public Function template() As template_proxy
            Return New template_proxy(Me)
        End Function
    End Class
End Class
