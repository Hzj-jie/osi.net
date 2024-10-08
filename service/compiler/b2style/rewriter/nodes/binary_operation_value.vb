
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class binary_operation_value
        Implements code_gen(Of typed_node_writer)

        Public Shared Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Dim function_name As String = _namespace.bstyle_format.operator_function_name(n.child(1).type_name)
            scope.current().call_hierarchy().to_bstyle_function(function_name)
            o.append(function_name)
            o.append("(")
            If Not code_gen_of(n.child(0)).build(o) Then
                Return False
            End If
            o.append(",")
            If Not code_gen_of(n.child(2)).build(o) Then
                Return False
            End If
            o.append(")")
            Return True
        End Function

        Private Function code_gen_build(ByVal n As typed_node,
                                ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            Return build(n, o)
        End Function
    End Class
End Class
