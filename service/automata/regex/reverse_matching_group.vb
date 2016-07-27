
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class rlexer
    Public Class reverse_matching_group
        Inherits matching_group_wrapper

        Private ReadOnly consumes As UInt32

        Public Sub New(ByVal g As matching_group)
            MyBase.New(g)
            Dim sg As string_matching_group = Nothing
            If cast(g, sg) AndAlso assert(Not sg Is Nothing) Then
                Me.consumes = sg.max_length
            ElseIf cast(g, DirectCast(Nothing, any_character_matching_group)) Then
                Me.consumes = uint32_1
            Else
                Me.consumes = uint32_0
            End If
        End Sub

        Public Overrides Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
            Dim r As vector(Of UInt32) = Nothing
            r = MyBase.match(i, pos)
            If r.null_or_empty() Then
                r.renew()
                If pos + consumes >= strlen(i) Then
                    r.emplace_back(strlen(i))
                Else
                    r.emplace_back(pos + consumes)
                End If
                Return r
            Else
                Return Nothing
            End If
        End Function
    End Class
End Class
