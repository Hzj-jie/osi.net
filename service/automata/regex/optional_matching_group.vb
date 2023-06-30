
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class rlexer
    Public Class optional_matching_group
        Inherits matching_group_wrapper

        Public Sub New(ByVal g As matching_group)
            MyBase.New(g)
        End Sub

        Public Overrides Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
            Dim r As vector(Of UInt32) = Nothing
            r = MyBase.match(i, pos)
            If r Is Nothing Then
                r = New vector(Of UInt32)()
            End If
            If r.empty() OrElse r.find(pos) = npos Then
                r.emplace_back(pos)
            End If
            Return r
        End Function
    End Class
End Class
