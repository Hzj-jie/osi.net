
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public NotInheritable Class bypass
        Inherits [default]

        Public Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Public Shared Sub register(ByVal l As rewriters, ByVal s As String)
            assert(Not l Is Nothing)
            assert(Not s.null_or_whitespace())
            l.register(s, New bypass(l))
        End Sub
    End Class
End Class
