
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class reinterpret_cast
        Implements code_gen(Of writer)

        Public Shared ReadOnly instance As New reinterpret_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            Return scope.current().variables().redefine(n.child(4).children_word_str(), n.child(2).children_word_str())
        End Function
    End Class
End Class
