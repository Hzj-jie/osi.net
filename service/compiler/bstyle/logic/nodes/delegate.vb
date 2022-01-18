
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class _delegate
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim ps As vector(Of String) = Nothing
            If n.child_count() = 5 Then
                ps = New vector(Of String)()
            Else
                ps = l.of_all_children(n.child(4)).dump()
            End If
            Return scope.current().delegates().define(n.child(1).children_word_str(),
                                                      n.child(2).children_word_str(),
                                                      builders.parameter_type.from(ps)) AndAlso
                   builders.of_callee_ref(n.child(2).children_word_str(),
                                          n.child(1).children_word_str(),
                                          ps).to(o)
        End Function
    End Class
End Class
