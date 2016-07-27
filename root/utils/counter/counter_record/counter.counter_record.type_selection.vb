
Imports osi.root.connector
Imports osi.root.constants.counter

Namespace counter
    Partial Friend Class counter_record
        Private Shared Sub valid_selector(ByVal b As Int16)
            If isdebugbuild() Then
                assert(_1count(b) = 1)
            End If
        End Sub

        Private Shared Sub [select](ByRef n As Int16, ByVal b As Int16)
            valid_selector(b)
            n = n Or b
        End Sub

        Private Shared Sub select_count(ByRef n As Int16)
            [select](n, counter_type.count)
        End Sub

        Private Shared Sub select_average(ByRef n As Int16)
            [select](n, counter_type.average)
        End Sub

        Private Shared Sub select_last_average(ByRef n As Int16)
            [select](n, counter_type.last_average)
        End Sub

        Private Shared Sub select_rate(ByRef n As Int16)
            [select](n, counter_type.rate)
        End Sub

        Private Shared Sub select_last_rate(ByRef n As Int16)
            [select](n, counter_type.last_rate)
        End Sub

        Private Function selected(ByVal b As Int16) As Boolean
            valid_selector(b)
            Return (type And b) > 0
        End Function

        Private Function count_selected() As Boolean
            Return selected(counter_type.count)
        End Function

        Private Function average_selected() As Boolean
            Return selected(counter_type.average)
        End Function

        Private Function last_average_selected() As Boolean
            Return selected(counter_type.last_average)
        End Function

        Private Function rate_selected() As Boolean
            Return selected(counter_type.rate)
        End Function

        Private Function last_rate_selected() As Boolean
            Return selected(counter_type.last_rate)
        End Function
    End Class
End Namespace
