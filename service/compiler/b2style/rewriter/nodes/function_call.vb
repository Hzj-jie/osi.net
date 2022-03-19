
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_call
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal name As String,
                              ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not n Is Nothing)
            assert(n.child_count() = 3 OrElse n.child_count() = 4)
            assert(Not o Is Nothing)
            If scope.current().variables().resolve(name, Nothing) Then
                ' This should be a delegate function call.
                Return l.of_all_children(n).build(o)
            End If

            If Not name.Contains(".") Then
                o.append(_namespace.bstyle_format.of(name))
                scope.current().call_hierarchy().to(name)
                For i As UInt32 = 1 To n.child_count() - uint32_1
                    If Not l.of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
                Return True
            End If

            Dim dot_pos As Int32 = name.LastIndexOf(".")
            ' dot is not allowed to be the first or last character.
            assert(dot_pos > 0 AndAlso dot_pos < name.Length() - 1)
            Dim function_name As String = _namespace.bstyle_format.in_global_namespace(name.Substring(dot_pos + 1))
            scope.current().call_hierarchy().to_bstyle_function(function_name)
            o.append(function_name)
            o.append("(")
            o.append(_namespace.bstyle_format.of(name.Substring(0, dot_pos)))
            If n.child_count() = 4 Then
                o.append(", ")
                If Not l.of(n.child(2)).build(o) Then
                    Return False
                End If
            End If
            o.append(")")
            Return True
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                             Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3 OrElse n.child_count() = 4)
            Return build(n.child(0).input_without_ignored(), n, o)
        End Function
    End Class
End Class
