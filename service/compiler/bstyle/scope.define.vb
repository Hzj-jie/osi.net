
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class define_t
            Private ReadOnly d As New unordered_set(Of String)()

            Public Sub define(ByVal s As String)
                assert(d.emplace(s).second())
            End Sub

            Public Function is_defined(ByVal s As String) As Boolean
                Return d.find(s) <> d.end()
            End Function
        End Class

        Public Function defines() As define_t
            Return from_root(Function(ByVal i As scope) As define_t
                                 assert(Not i Is Nothing)
                                 Return i.d
                             End Function)
        End Function
    End Class
End Class
