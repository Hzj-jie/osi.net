
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_clause
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_clause)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            If Not l.of(n.child(0)).build(o) Then
                o.err("@value-clause name ", n.child(0))
                Return False
            End If
            Using logic_gen_of(Of value).with_current_target(n.child(0))
                If Not l.of(n.child(2)).build(o) Then
                    o.err("@value-clause value ", n.child(2))
                    Return False
                End If
            End Using
            Return True
        End Function
    End Class
End Class
