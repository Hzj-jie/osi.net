
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class param
        Inherits rewriter_wrapper
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of param)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2 OrElse n.child_count() = 3)
            If Not l.of(n.child(0)).build(o) Then
                Return False
            End If
            ' Do not want to randomly bypass &.
            If n.child_count() = 3 Then
                o.append(n.child(1))
            End If
            If Not l.of(n.last_child()).build(o) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Class
