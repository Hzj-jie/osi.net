
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class rlexer
    Public Class matching_group_wrapper
        Implements matching_group

        Private ReadOnly g As matching_group

        Protected Sub New(ByVal g As matching_group)
            assert(g IsNot Nothing)
            Me.g = g
        End Sub

        Public Overridable Function match(ByVal i As String,
                                          ByVal pos As UInt32) As vector(Of UInt32) Implements matching_group.match
            Return g.match(i, pos)
        End Function

        Public Shared Operator +(ByVal this As matching_group_wrapper) As matching_group
            Return If(this Is Nothing, Nothing, this.g)
        End Operator
    End Class
End Class
