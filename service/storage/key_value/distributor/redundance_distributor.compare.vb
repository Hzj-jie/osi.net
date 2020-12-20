
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Public Class redundance_distributor
    'since there is no 'cut' similar functionality in the system,
    'so if one has less data than the other, it shows partial data has been lost
    Private Shared Function less(ByVal lv() As Byte,
                                 ByVal lt As Int64,
                                 ByVal rv() As Byte,
                                 ByVal rt As Int64) As Boolean
        Return lt < rt OrElse
               (lt = rt AndAlso array_size(lv) < array_size(rv))
    End Function

    Private Shared Function equal(ByVal lv() As Byte,
                                  ByVal lt As Int64,
                                  ByVal rv() As Byte,
                                  ByVal rt As Int64) As Boolean
        Return lt = rt AndAlso array_size(lv) = array_size(rv)
    End Function

    Private Shared Function valid_read_output(ByVal buff() As Byte, ByVal ts As Int64) As Boolean
        Return ts >= npos AndAlso
               (Not ((ts = npos) Xor (isemptyarray(buff))))
    End Function

    'assert the input is valid, i.e. the size of values and ts are the same and also > 1
    'return false if no value / ts is valid,
    'in this case, nodes_results / best_value / best_ts are not reliable
    'return true if selected the best_value / best_ts from the input values / ts,
    'in this case, array_size(nodes_results) == array_size(ts), and at least one element in the array is true
    'best_value and best_ts are set
    Private Shared Function compare(ByVal ecs() As event_comb,
                                    ByVal values() As ref(Of Byte()),
                                    ByVal tss() As ref(Of Int64),
                                    ByRef nodes_results() As Boolean,
                                    ByRef best_value() As Byte,
                                    ByRef best_ts As Int64) As Boolean
        assert(array_size(ecs) > 1)
        assert(array_size(ecs) = array_size(values))
        assert(array_size(ecs) = array_size(tss))
        ReDim nodes_results(array_size(tss) - 1)
        Dim i As Int32 = 0
        For i = 0 To array_size(tss) - 1
            If ecs(i).end_result() AndAlso valid_read_output(+(values(i)), +(tss(i))) Then
                best_value = +(values(i))
                best_ts = +(tss(i))
                Exit For
            End If
        Next
        If i = array_size(tss) Then
            Return False
        Else
            For j As Int32 = 0 To i - 1
                nodes_results(j) = False
            Next
            nodes_results(i) = True
            For j As Int32 = i + 1 To array_size(tss) - 1
                If ecs(j).end_result() AndAlso valid_read_output(+(values(j)), +(tss(j))) Then
                    If equal(+(values(j)), +(tss(j)), best_value, best_ts) Then
                        nodes_results(j) = True
                    ElseIf less(best_value, best_ts, +(values(j)), +(tss(j))) Then
                        best_value = +(values(j))
                        best_ts = +(tss(j))
                        nodes_results(j) = True
                        For k As Int32 = i To j - 1
                            nodes_results(k) = False
                        Next
                    Else
                        nodes_results(j) = False
                    End If
                Else
                    nodes_results(j) = False
                End If
            Next
#If DEBUG Then
            For i = 0 To array_size(tss) - 1
                If nodes_results(i) Then
                    Exit For
                End If
            Next
            assert(i < array_size(tss))
#End If
            Return True
        End If
    End Function
End Class
