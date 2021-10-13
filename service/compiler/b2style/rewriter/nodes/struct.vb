
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class struct
        Inherits rewriter_wrapper
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of struct)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 2)
            o.append(n.child(0))
            If Not l.of(n.child(1)).build(o) Then
                Return False
            End If
            For i As UInt32 = 2 To n.child_count() - uint32_1
                o.append(n.child(i))
            Next
            Return True
        End Function
    End Class
End Class
