
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public NotInheritable Class bypass
        Inherits [default]

        Private Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Public Shared Function registerer(ByVal s As String) As Action(Of rewriters)
            Return Sub(ByVal l As rewriters)
                       assert(Not l Is Nothing)
                       assert(Not s.null_or_whitespace())
                       l.register(s, New bypass(l))
                   End Sub
        End Function
    End Class
End Class
