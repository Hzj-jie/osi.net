
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class self_value_clause
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal i As code_gens(Of typed_node_writer))
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of self_value_clause)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(n.child_count() = 3)
            If Not l.of(n.child(0)).build(o) Then
                Return False
            End If
            Dim function_name As String = operations.self_function_name(n.child(1))
            scope.current().call_hierarchy().to(function_name)
            o.append("=").append(function_name).append("(")
            If Not l.of(n.child(0)).build(o) Then
                Return False
            End If
            o.append(",")
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            o.append(")")
            Return True
        End Function
    End Class
End Class