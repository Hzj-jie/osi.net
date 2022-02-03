
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class _function
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim function_name As String = _namespace.bstyle_format.of(n.child(1).input_without_ignored())
            Using scope.current().start_scope().current_function().define(function_name)
                Dim fo As New typed_node_writer()
                Return l.of_all_children(n).build(fo) AndAlso
                       o.append(scope.current().call_hierarchy().filter(function_name, AddressOf fo.ToString))
            End Using
        End Function
    End Class
End Class
