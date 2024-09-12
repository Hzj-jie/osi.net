
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Partial Private NotInheritable Class template_type_name
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not o Is Nothing)
            Dim extended_type As String = Nothing
            If Not scope.template_t.resolve(n, extended_type) Then
                Return False
            End If
            Return o.append(extended_type)
        End Function
    End Class
End Class

