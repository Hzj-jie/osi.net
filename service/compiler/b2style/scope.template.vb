﻿
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
            Private ReadOnly m As New unordered_map(Of String, definition)()
            Private ReadOnly cr As New class_reparser(m)

            Private NotInheritable Class definition
                Public ReadOnly template As template_template
                Public ReadOnly injected_types As New unordered_set(Of vector(Of String))()
                Public ReadOnly injector As New typed_node_writer()

                Public Sub New(ByVal t As template_template)
                    assert(Not t Is Nothing)
                    template = t
                End Sub
            End Class

            Private NotInheritable Class class_reparser
                Inherits code_gens(Of typed_node_writer).reparser(Of b2style.parser)

                Private ReadOnly m As unordered_map(Of String, definition)

                Public Sub New(ByVal m As unordered_map(Of String, definition))
                    assert(Not m Is Nothing)
                    Me.m = m
                End Sub

                Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
                    assert(Not n Is Nothing)
                    assert(n.child_count() = 4)
                    Dim name As String = template_type_name_name(n)
                    ' The definition should be searched already in resolve.
                    Dim d As definition = +m.find_opt(name)
                    assert(Not d Is Nothing)
                    Dim types As vector(Of String) = Nothing
                    If Not template_type_name_types(n, types) Then
                        Return False
                    End If
                    ' TODO: Should resolve type-aliases.
                    If Not d.injected_types.emplace(types).second() Then
                        ' Injected already.
                        s = ""
                        Return True
                    End If
                    Return d.template.apply(types, s)
                End Function
            End Class

            Public Shared Function template_type_name_name(ByVal n As typed_node) As String
                assert(Not n Is Nothing)
                assert(n.child_count() = 4)
                Return template_template.template_name(n.child(0), n.child(2).child_count())
            End Function

            Private Shared Function template_type_name_types(ByVal n As typed_node,
                                                             ByRef o As vector(Of String)) As Boolean
                assert(Not n Is Nothing)
                assert(n.child_count() = 4)
                Return b2style.code_builder.current().code_gens.of_all_children(n.child(2)).dump(o)
            End Function

            Public Function define(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                assert(Not n Is Nothing)
                assert(Not o Is Nothing)
                Dim t As template_template = Nothing
                If Not template_template.of(n, t) Then
                    Return False
                End If
                assert(Not t Is Nothing)
                Dim d As New definition(t)
                If Not m.emplace(t.name(), d).second() Then
                    raise_error(error_type.user, "Template ", t.name(), " has been defined already.")
                    Return False
                End If
                o.append(d.injector)
                Return True
            End Function

            Public Function resolve(ByVal n As typed_node, ByRef extended_class_name As String) As Boolean
                assert(Not n Is Nothing)
                Dim name As String = template_type_name_name(n)
                Dim d As definition = Nothing
                If Not m.find(name, d) Then
                    Return False
                End If
                assert(Not d Is Nothing)
                If Not cr.build(n, d.injector) Then
                    Return False
                End If
                Dim types As vector(Of String) = Nothing
                If Not template_type_name_types(n, types) Then
                    Return False
                End If
                extended_class_name = d.template.extended_type_name(types)
                Return True
            End Function
        End Class

        Public Structure template_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                Return s.t.define(n, o)
            End Function

            Public Function resolve(ByVal n As typed_node, ByRef extended_class_name As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.t.resolve(n, extended_class_name) Then
                        Return True
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user,
                            "Template ",
                            template_t.template_type_name_name(n),
                            " has not been defined.")
                Return False
            End Function
        End Structure

        Public Function template() As template_proxy
            Return New template_proxy(Me)
        End Function
    End Class
End Class
