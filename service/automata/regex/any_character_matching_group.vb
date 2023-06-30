
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class rlexer
    Public Class any_character_matching_group
        Implements matching_group

        Public Function match(ByVal i As String,
                              ByVal pos As UInt32) As vector(Of UInt32) Implements matching_group.match
            If strlen(i) <= pos Then
                Return Nothing
            Else
                Dim r As vector(Of UInt32) = Nothing
                r = New vector(Of UInt32)()
                r.emplace_back(pos + 1)
                Return r
            End If
        End Function
    End Class
End Class
