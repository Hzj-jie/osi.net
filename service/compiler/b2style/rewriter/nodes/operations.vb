
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Public NotInheritable Class operations
        Private ReadOnly l As rewriters

        Public Sub New(ByVal l As rewriters)
            assert(Not l Is Nothing)
            Me.l = l
        End Sub

        Public Function function_name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            assert(Not n.type_name.null_or_whitespace())
            Return l.code_gen_of(Of kw_namespace)().bstyle_format(strcat("::b2style::", n.type_name.Replace("-"c, "_"c)))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
