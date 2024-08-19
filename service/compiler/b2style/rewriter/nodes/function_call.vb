
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class function_call
        Implements code_gen(Of typed_node_writer)

        Public Shared Function build_struct_function(ByVal this As String, ByVal name As String) As String
            assert(Not this.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            Return String.Concat(this, ".", name)
        End Function

        Public Shared Function split_struct_function(ByVal name As String,
                                                     ByRef o As tuple(Of String, String)) As Boolean
            assert(Not name.null_or_whitespace())
            If Not name.Contains(".") Then
                Return False
            End If
            Dim dot_pos As Int32 = name.LastIndexOf(".")
            ' dot is not allowed to be the first or last character.
            assert(dot_pos > 0 AndAlso dot_pos < name.Length() - 1)
            o = tuple.emplace_of(name.Substring(0, dot_pos), name.Substring(dot_pos + 1))
            Return True
        End Function

        Public Shared Function build(ByVal name As String,
                                     ByVal n As typed_node,
                                     ByVal o As typed_node_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not n Is Nothing)
            assert(n.child_count() = 3 OrElse n.child_count() = 4)
            assert(Not o Is Nothing)

            Dim p As tuple(Of String, String) = Nothing
            If Not split_struct_function(name, p) Then
                If scope.current().variables().try_resolve(name, Nothing) Then
                    ' This should be a delegate function call.
                    Return code_gens().of_all_children(n).build(o)
                End If

                o.append(_namespace.bstyle_format.of(name))
                scope.current().call_hierarchy().to(name)
                For i As UInt32 = 1 To n.child_count() - uint32_1
                    If Not code_gen_of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
                Return True
            End If

            Dim function_name As String = _namespace.bstyle_format.fully_qualified_name(p.second())
            ' TODO: This never works, class variables are not defined in scope.current().variables().
            ' If scope.current().variables().resolve(function_name, Nothing) Then
            '     Return code_gens().of_all_children(n).build(o)
            ' End If

            scope.current().call_hierarchy().to_bstyle_function(function_name)
            o.append(function_name)
            o.append("(")
            o.append(_namespace.bstyle_format.of(p.first()))
            If n.child_count() = 4 Then
                o.append(", ")
                If Not code_gen_of(n.child(2)).build(o) Then
                    Return False
                End If
            End If
            o.append(")")
            Return True
        End Function

        Private Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                             Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3 OrElse n.child_count() = 4)
            Return build(n.child(0).input_without_ignored(), n, o)
        End Function
    End Class
End Class
