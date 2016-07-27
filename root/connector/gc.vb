
Public Module _gc
    Public Sub waitfor_gc_collect()
        For i As Int32 = 0 To GC.MaxGeneration()
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Next
    End Sub

    Public Sub gc_trigger()
        While True
            waitfor_gc_collect()
            sleep_seconds(10)
        End While
    End Sub

    Public Function waitfor_gc_collect_until(ByVal check As Func(Of Boolean),
                                             Optional ByVal round As UInt32 = 1000) As Boolean
        assert(Not check Is Nothing)
        While round > 0
            round -= 1
            If check() Then
                Return True
            Else
                waitfor_gc_collect()
            End If
        End While
        Return False
    End Function

    Public Function waitfor_gc_collect_when(ByVal check As Func(Of Boolean),
                                            Optional ByVal round As UInt32 = 1000) As Boolean
        assert(Not check Is Nothing)
        Return waitfor_gc_collect_until(Function() As Boolean
                                            Return Not check()
                                        End Function,
                                        round)
    End Function
End Module
