
Imports osi.root.connector

Partial Public Class bt(Of T)
    Partial Protected Friend Class node
        Public Function compare(ByVal that As node) As Int32
            Return compare(If(that Is Nothing, DirectCast(Nothing, T), that.value()))
        End Function

        Public Function compare(ByVal that As T) As Int32
            Return cmp(value(), that)
        End Function

        Public Shared Operator +(ByVal this As node) As T
            Return If(this Is Nothing, Nothing, this.value())
        End Operator

        Public Function replace_by(ByVal b As node) As node
            Return replace_by(Me, b)
        End Function

        Public Function replace_by_left_subtree() As node
            Return replace_by_left_subtree(Me)
        End Function

        Public Function replace_by_right_subtree() As node
            Return replace_by_right_subtree(Me)
        End Function

        Public Function replace_by_left_max() As node
            Return replace_by_left_max(Nothing)
        End Function

        Public Function replace_by_left_max(ByRef parent_of_replaced_node As node) As node
            Return replace_by_left_max(Me, parent_of_replaced_node)
        End Function

        Public Function replace_by_right_min() As node
            Return replace_by_right_min(Nothing)
        End Function

        Public Function replace_by_right_min(ByRef parent_of_replaced_node As node) As node
            Return replace_by_right_min(Me, parent_of_replaced_node)
        End Function

        Public Function [erase]() As node
            Return [erase](Nothing)
        End Function

        Public Function [erase](ByRef parent_of_removed_node As node) As node
            Return [erase](rnd_bool(), parent_of_removed_node)
        End Function

        Public Function [erase](ByVal select_left As Boolean, ByRef parent_of_removed_node As node) As node
            Return [erase](Me, select_left, parent_of_removed_node)
        End Function
    End Class
End Class
