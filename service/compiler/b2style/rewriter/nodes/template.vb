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
    Public NotInheritable Class template
        Implements code_gen(Of typed_node_writer)
        ' TODO: Remove
        Public Shared Function name_of(ByVal name As String, ByVal type_count As UInt32) As String
            assert(Not name.null_or_whitespace())
            assert(type_count > 0)
            Return String.Concat(name, "__", type_count)
        End Function

        Public Shared Function name_of(ByVal n As typed_node, ByVal type_count As UInt32) As String
            assert(Not n Is Nothing)
            Return name_of(n.input_without_ignored(), type_count)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Dim name As String = Nothing
            Dim t As scope.template_template = Nothing
            Return [of](code_gens(), n, name, t) AndAlso
                   scope.current().template().define(name, t)
        End Function

        Public Shared Function resolve(ByVal n As typed_node, ByRef extended_type_name As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            Dim types As vector(Of String) = code_gens().of_all_children(n.child(2)).dump()
            Dim name As String = Nothing
            If Not code_gens().typed(Of scope.template_t.name)(n.type_name).of(n, name) Then
                raise_error(error_type.user, "Cannot retrieve template name of ", n.input())
                Return False
            End If
            Return scope.current().template().resolve(name, types, extended_type_name, lazier.of(AddressOf n.input))
        End Function

        Public Shared Function [of](ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef name As String,
                                    ByRef o As scope.template_template) As Boolean
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            assert(n.child_count() = 2)
            Dim name_node As typed_node = Nothing
            If Not l.typed(Of scope.template_t.name)(n.child(1).child().type_name).of(n, name) OrElse
               Not scope.template_builder.name_node_of(n, name_node) Then
                Return False
            End If
            assert(Not name_node Is Nothing)
            Dim type_param_list As vector(Of String) = scope.template_builder.type_param_list(n)
            assert(Not type_param_list.null_or_empty())
            Dim body As typed_node = n.child(1)
            assert(Not body Is Nothing)
            assert(body.type_name.Equals("template-body"))
            assert(body.child_count() = 1)
            If type_param_list.size() >
               type_param_list.stream().collect_by(stream(Of String).collectors.unique()).size() Then
                raise_error(error_type.user,
                            "Template ",
                            name_node.input_without_ignored(),
                            " has duplicated template type parameters: [",
                            type_param_list.str(", "),
                            "]")
                Return False
            End If
            o = New scope.template_template(body.child(), name_node, type_param_list)
            Return True
        End Function

        ' @VisibleForTesting
        Public Shared Function [of](ByVal l As code_gens(Of typed_node_writer),
                                    ByVal n As typed_node,
                                    ByRef o As scope.template_template) As Boolean
            Return [of](l, n, Nothing, o)
        End Function
    End Class
End Class
