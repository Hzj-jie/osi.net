
Imports osi.root.formation

Partial Public Class rlexer
    Public Class unmatched_matching_group
        Inherits matching_group_wrapper

        Public Sub New(ByVal g As matching_group)
            MyBase.New(g)
        End Sub

        Public Overrides Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
            Dim v As vector(Of UInt32) = Nothing
            v = MyBase.match(i, pos)
            If v.null_or_empty() Then
                v.renew()
                v.emplace_back(pos)
                Return v
            Else
                Return Nothing
            End If
        End Function
    End Class
End Class
