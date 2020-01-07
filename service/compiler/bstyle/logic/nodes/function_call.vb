
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class function_call
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of function_call)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() > 3)
            If n.child_count() = 3 Then
                builders.of_caller(n.child(0).word().str(), value.current_target(), vector.of(Of String)()).to(o)
                Return True
            End If
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using targets As read_scoped(Of vector(Of String)).ref = value_list.current_targets()
                builders.of_caller(n.child(0).word().str(), value.current_target(), +targets).to(o)
                Return True
            End Using
        End Function
    End Class
End Class
