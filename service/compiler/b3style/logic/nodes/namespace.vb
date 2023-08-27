
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class _namespace
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 4)
            Using scope.current().current_namespace().define(scope.current_namespace_t.of(n.child(1).word().str()))
                For i As UInt32 = 3 To n.child_count() - uint32_2
                    If Not code_gen_of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
            End Using
            Return True
        End Function
    End Class
End Class
