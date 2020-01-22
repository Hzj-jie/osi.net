
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [while]
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [while])()
        End Sub

        Private Function while_value(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return l.of(n.child(2)).build(o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 5)
            If Not while_value(n, o) Then
                Return False
            End If
            Using value_target As read_scoped(Of String).ref = code_gen_of(Of value)().read_target()
                Return builders.of_while_then(+value_target,
                                              Function() As Boolean
                                                  Return l.of(n.child(4)).build(o) AndAlso while_value(n, o)
                                              End Function).to(o)
            End Using
        End Function
    End Class
End Class
