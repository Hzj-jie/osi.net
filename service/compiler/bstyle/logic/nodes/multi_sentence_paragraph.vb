
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class multi_sentence_paragraph
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return builders.start_scope(o).of(
                       Function() As Boolean
                           Using scope.current().start_scope()
                               Dim i As UInt32 = 1
                               While i < n.child_count() - uint32_1
                                   If Not code_gen_of(n.child(i)).build(o) Then
                                       Return False
                                   End If
                                   i += uint32_1
                               End While
                           End Using
                           Return True
                       End Function)
        End Function
    End Class
End Class
