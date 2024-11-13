
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class template_t
        Private ReadOnly m As New unordered_map(Of name_with_namespace, definition)()

        Private NotInheritable Class definition
            Private ReadOnly template As template_template
            Private ReadOnly injected_types As New unordered_set(Of vector(Of String))()

            Public Sub New(ByVal t As template_template)
                assert(Not t Is Nothing)
                template = t
            End Sub

            Public Function apply(ByVal types As vector(Of String), ByRef o As String) As Boolean
                assert(Not types.null_or_empty())
                If current().features().with_type_alias() Then
                    types = types.stream().
                                  map(AddressOf normalized_type.parameter_type_of).
                                  map(AddressOf builders.parameter_type.full_type).
                                  collect_to(Of vector(Of String))()
                End If
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

        Public Function define(ByVal name As String, ByVal t As template_template) As Boolean
            assert(Not name.null_or_whitespace())
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
                         scope(Of T).current().current_namespace().define(name.namespace()))
                    If Not code_build(s, scope(Of T).current().root_type_injector().current()) Then
                        Return ternary.false
                    End If
                End Using
            End If
            extended_type_name = namespace_t.fully_qualified_name(
                                     name.namespace(),
                                     d.extended_type_name(types))
            Return ternary.true
        End Function

        Public Interface name
            Function [of](ByVal n As typed_node, ByRef o As String) As Boolean
        End Interface

        Public Interface name_node
            Function [of](ByVal n As typed_node, ByRef o As typed_node) As Boolean
        End Interface

        Public Shared Function resolve(ByVal n As typed_node, ByRef extended_type_name As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Dim types As vector(Of String) = code_gens().of_all_children(n.child(2)).dump()
            Dim name As String = Nothing
            If Not code_gens().typed(Of template_t.name)(n).of(n, name) Then
                raise_error(error_type.user, "Cannot retrieve template name of ", n.input())
                Return False
            End If
            Return scope(Of T).current().
                               template().
                               resolve(name, types, extended_type_name, lazier.of(AddressOf n.input))
        End Function

        Public Shared Function type_param_count(ByVal n As typed_node) As UInt32
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            n = n.child(0)
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            assert(Not n.child(2).leaf())
            Return n.child(2).child_count()
        End Function

        Public Shared Function type_param_list(ByVal n As typed_node) As vector(Of String)
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            n = n.child(0)
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Return code_gens().of_all_children(n.child(2)).dump()
        End Function

        Public Shared Function body_of(ByVal n As typed_node) As typed_node
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Return n.child(1).child()
        End Function

        Public Shared Function name_node_of(ByVal n As typed_node, ByRef o As typed_node) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            n = n.child(1).child()
            Return code_gens().typed(Of template_t.name_node)(n).of(n, o)
        End Function

        Public Shared Function name_node_of(ByVal n As typed_node) As typed_node
            Dim o As typed_node = Nothing
            assert(name_node_of(n, o))
            Return o
        End Function

        Public Shared Function name_of(ByVal n As typed_node) As String
            Return name_of(name_node_of(n), type_param_count(n))
        End Function

        Public Shared Function name_of(ByVal name As String, ByVal type_count As UInt32) As String
            assert(Not name.null_or_whitespace())
            assert(type_count > 0)
            Return String.Concat(name, "__", type_count)
        End Function

        Public Shared Function name_of(ByVal n As typed_node, ByVal type_count As UInt32) As String
            assert(Not n Is Nothing)
            Return name_of(n.input_without_ignored(), type_count)
        End Function

        ' @VisibleForTesting
        Public Shared Function [of](ByVal l As code_gens(Of WRITER),
                                    ByVal n As typed_node,
                                    ByRef o As template_template) As Boolean
            Return [of](l, n, Nothing, o)
        End Function

        Public Shared Function [of](ByVal n As typed_node,
                                    ByRef name As String,
                                    ByRef o As template_template) As Boolean
            Return [of](code_gens(), n, name, o)
        End Function

        Private Shared Function [of](ByVal l As code_gens(Of WRITER),
                                     ByVal n As typed_node,
                                     ByRef name As String,
                                     ByRef o As template_template) As Boolean
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Dim name_node As typed_node = Nothing
            If Not l.typed(Of template_t.name)(n.child(1).child()).of(n, name) OrElse
               Not name_node_of(n, name_node) Then
                Return False
            End If
            assert(Not name_node Is Nothing)
            Dim types As vector(Of String) = type_param_list(n)
            assert(Not types.null_or_empty())
            Dim body As typed_node = n.child(1)
            assert(Not body Is Nothing)
            assert(body.type_name.Equals("template-body"))
            assert(body.child_count() = 1)
            If types.size() >
               types.stream().collect_by(stream(Of String).collectors.unique()).size() Then
                raise_error(error_type.user,
                            "Template ",
                            name_node.input_without_ignored(),
                            " has duplicated template type parameters: [",
                            types.str(", "),
                            "]")
                Return False
            End If
            o = New template_template(body.child(), name_node, types)
            Return True
        End Function
    End Class

    Public Structure template_proxy
        Public Function define(ByVal name As String, ByVal t As template_template) As Boolean
            Return scope(Of T).current().myself().template().define(name, t)
        End Function

        Public Function resolve(ByVal name As String,
                                ByVal types As vector(Of String),
                                ByRef extended_type_name As String,
                                ByVal msg As Object) As Boolean
            Dim s As scope(Of WRITER, __BUILDER, __CODE_GENS, T) = scope(Of T).current()
            While Not s Is Nothing
                Dim r As ternary = s.myself().
                                     template().
                                     resolve(name, types, extended_type_name)
                If Not r.unknown_() Then
                    Return r
                End If
                s = s.parent
            End While
            raise_error(error_type.user,
                        "Template [",
                        name,
                        "] has not been defined for ",
                        msg)
            Return False
        End Function
    End Structure
End Class
