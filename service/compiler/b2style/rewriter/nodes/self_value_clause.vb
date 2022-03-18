
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Private NotInheritable Class self_value_clause
        Implements code_gen(Of typed_node_writer)

        Private Const self_prefix As String = "self-"
        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(n.child_count() = 3)
            If Not l.of(n.child(0)).build(o) Then
                Return False
            End If
            Dim function_name As String = l.of(n.child(1)).dump()
            assert(function_name.StartsWith(self_prefix))
            function_name = _namespace.bstyle_format.operator_function_name(
                                function_name.Substring(self_prefix.Length()))
            scope.current().call_hierarchy().to_bstyle_function(function_name)
            o.append("=")
            o.append(function_name)
            o.append("(")
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