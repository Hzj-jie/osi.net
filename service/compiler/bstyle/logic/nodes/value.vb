
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(b IsNot Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(n IsNot Nothing)
            assert(o IsNot Nothing)
            assert(strsame(n.type_name, "value") OrElse
                   (strsame(n.type_name, "ignore-result-function-call") AndAlso
                    strsame(n.child().type_name, "function-call")))
            Return l.of(n.child()).build(o)
        End Function
    End Class
End Class
