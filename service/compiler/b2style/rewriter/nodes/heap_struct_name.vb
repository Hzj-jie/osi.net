
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class heap_struct_name
        Implements code_gen(Of typed_node_writer)

        Public Shared ReadOnly instance As New heap_struct_name()

        Private Sub New()
        End Sub

        Private Shared Function bstyle_format(ByVal n As typed_node) As String
            assert(n.child_count() = 3)
            assert(n.child(0).child_count() = 4)
            Return streams.of(n.child(0).child(0),
                              n.child(1),
                              n.child(2),
                              n.child(0).child(1),
                              n.child(0).child(2),
                              n.child(0).child(3)).
                           map(Function(ByVal x As typed_node) As String
                                   assert(Not x Is Nothing)
                                   Return x.children_word_str()
                               End Function).
                           collect_by(stream(Of String).collectors.to_str("")).
                           ToString()
        End Function

        Public Shared Function bstyle_function(ByVal n As typed_node) As String
            Dim s As String = bstyle_format(n)
            Dim start As Int32 = s.LastIndexOf(".")
            Dim e As Int32 = s.LastIndexOf("[")
            assert(start > 0 AndAlso start < e)
            Dim last_part As String = s.Substring(start, e - start)
            Return strcat(s.Remove(start, e - start), last_part)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not o Is Nothing)
            o.append(bstyle_format(n))
            Return True
        End Function
    End Class
End Class
