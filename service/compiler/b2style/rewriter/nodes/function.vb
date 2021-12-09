
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class [function]
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of [function])()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using New scope_wrapper()
                Dim function_name As String = namespace_.bstyle_format(n.child(1).word().str())
                scope.current().current_function().define(function_name)
                Dim fo As New typed_node_writer()
                If Not code_gens(Of typed_node_writer).default.build_all_children(l, n, fo) Then
                    Return False
                End If
                o.append(scope.current().call_hierarchy().filter(function_name, AddressOf fo.dump))
                Return True
            End Using
        End Function
    End Class
End Class
