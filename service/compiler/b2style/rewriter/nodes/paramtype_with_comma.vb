
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class paramtype_with_comma
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(b IsNot Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(n IsNot Nothing)
            assert(o IsNot Nothing)
            If n.descentdant_of("template-type-name") OrElse
               n.descentdant_of("function-name-with-template") Then
                ' Expect to dump only the type name.
                Return l.of(n.child(0)).build(o)
            End If
            Return l.of_all_children(n).build(o)
        End Function
    End Class
End Class
