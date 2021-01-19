
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class garbage_collector
    Public Shared Sub waitfor_collect()
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    ' Repeats waitfor_collect() for a magic eight times.
    Public Shared Sub repeat_collect(Optional ByVal round As UInt32 = 8)
        For i As Int32 = 0 To CInt(round) - 1
            waitfor_collect()
        Next
    End Sub

    Public Shared Sub trigger()
        While True
            waitfor_collect()
            sleep_seconds(10)
        End While
    End Sub

    Public Shared Function waitfor_collect_until(ByVal check As Func(Of Boolean),
                                                 Optional ByVal round As UInt32 = 1000) As Boolean
        assert(Not check Is Nothing)
        While round > 0
            round -= uint32_1
            If check() Then
                Return True
            End If
            waitfor_collect()
        End While
        Return False
    End Function

    Public Shared Function waitfor_collect_when(ByVal check As Func(Of Boolean),
                                                Optional ByVal round As UInt32 = 1000) As Boolean
        assert(Not check Is Nothing)
        Return waitfor_collect_until(Function() As Boolean
                                         Return Not check()
                                     End Function,
                                     round)
    End Function

    Public Shared Function force_aggressive_collecting() As IDisposable
        GC.AddMemoryPressure(max_int32)
        Return defer.to(Sub()
                                GC.RemoveMemoryPressure(max_int32)
                            End Sub)
    End Function

    Private Sub New()
    End Sub
End Class
