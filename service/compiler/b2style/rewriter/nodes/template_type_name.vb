
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class template_type_name
        Implements code_gen(Of typed_node_writer), template.name

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not o Is Nothing)
            Dim extended_type As String = Nothing
            If Not template.resolve(n, extended_type) Then
                Return False
            End If
            Return o.append(_namespace.bstyle_format.of(extended_type))
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean Implements template.name.of
            assert(Not n Is Nothing)
            assert(n.child_count() = 4)
            o = template.name_of(n.child(0), n.child(2).child_count())
            Return True
        End Function
    End Class
End Class

