
Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.service.storage

Public Class virtdisk_test
    Inherits repeat_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New virtdisk_case(), If(isdebugbuild(), 1, 2) * 65536)
    End Sub

    Private Class virtdisk_case
        Inherits event_comb_case

        Private ReadOnly vd As virtdisk

        Public Sub New()
            vd = virtdisk.memory_virtdisk()
        End Sub

        Private Shared Sub rnd_data(ByRef start As UInt64, ByRef len As UInt32, ByRef buff() As Byte)
            start = rnd_int(0, 8192 * 8192 + 1)
            len = rnd_int(1024, 1024 * 16 + 1)
            buff = next_bytes(len)
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
                                      assertion.is_true(ec.end_result())
                                      p = New pointer(Of Byte())()
                                      ec = vd.read(start, len, p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      assertion.is_not_null(+p)
                                      assertion.equal(array_size(+p), len)
                                      assertion.equal(memcmp(+p, buff), 0)
                                      Return goto_end()
                                  End Function)
        End Function

        Public Overrides Function finish() As Boolean
            async_sync(vd.clear())
            async_sync(vd.shrink_to_fit())
            Return MyBase.finish()
        End Function
    End Class
End Class
