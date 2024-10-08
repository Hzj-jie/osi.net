
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class logic
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            o.append(n.child(1).word().str().Trim(character.quote))
            o.append(newline.incode())
            Return True
        End Function
    End Class
End Class
