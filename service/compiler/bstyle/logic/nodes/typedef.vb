
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class typedef
        Implements code_gen(Of writer)

        Private Shared ReadOnly instance As New typedef()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Private Shared Function get_type(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            If n.child().type_name.Equals("string") Then
                Return n.child().word().str().Trim(character.quote).c_unescape()
            End If
            If n.child().type_name.Equals("name") Then
                Return n.child().word().str()
            End If
            assert(False)
            Return Nothing
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Return scope.current().type_alias().define(get_type(n.child(2)), get_type(n.child(1)))
        End Function
    End Class
End Class
