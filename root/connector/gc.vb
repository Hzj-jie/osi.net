
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Module _gc
    Public Sub waitfor_gc_collect()
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    ' Repeats waitfor_gc_collect() for a magic eight times.
    Public Sub repeat_gc_collect(Optional ByVal round As UInt32 = 8)
        For i As Int32 = 0 To CInt(round) - 1
            waitfor_gc_collect()
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
            round -= uint32_1
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

    Public Sub safe_finalize(Of T)(ByVal this As T, ByVal f As Action(Of T))
        assert(Not this Is Nothing)
        assert(Not f Is Nothing)
        Try
            f(this)
        Catch
        End Try
        GC.KeepAlive(this)
    End Sub

    Public Sub safe_finalize(Of T)(ByVal this As T, ByVal f As Action)
        assert(Not this Is Nothing)
        assert(Not f Is Nothing)
        Try
            f()
        Catch
        End Try
        GC.KeepAlive(this)
    End Sub
End Module
