
Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.service.storage

Public Class virtdisk_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New repeat_event_comb_case_wrapper(New virtdisk_perf_case(),
                                                      If(isdebugbuild(), 1, 8) * 65536),
                   undetermined_max_loops)
    End Sub

    Private Class virtdisk_perf_case
        Inherits event_comb_case

        Private ReadOnly vd As virtdisk

        Public Sub New()
            vd = virtdisk.memory_virtdisk()
        End Sub

        Private Shared Sub rnd_data(ByRef start As UInt64, ByRef len As UInt32, ByRef buff() As Byte)
            start = rnd_int(0, 8192 * 8192 + 1)
            len = rnd_int(1024, 1024 * 16 + 1)
            ReDim buff(len - 1)
        End Sub

        Public Overrides Function create() As event_comb
            Dim ec As event_comb = Nothing
            Dim start As UInt64 = 0
            Dim len As UInt32 = 0
            Dim buff() As Byte = Nothing
            Dim p As pointer(Of Byte()) = Nothing
            Return New event_comb(Function() As Boolean
                                      rnd_data(start, len, buff)
                                      ec = vd.write(start, len, buff)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      p = New pointer(Of Byte())()
                                      ec = vd.read(start, len, p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      assert_not_nothing(+p)
                                      assert_equal(array_size(+p), len)
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
