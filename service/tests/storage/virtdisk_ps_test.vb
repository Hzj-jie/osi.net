
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.storage

Public Class virtdisk_ps_test
    Inherits multi_procedure_case_wrapper

    Public Sub New()
        MyBase.New(New repeat_event_comb_case_wrapper(New virtdisk_ps_case(),
                                                      If(isdebugbuild(), 1, 2) * 65536),
                   If(isdebugbuild(), 1, 2) * 8)
    End Sub

    Private Class virtdisk_ps_case
        Inherits event_comb_case

        Private ReadOnly vd As virtdisk

        Public Sub New()
            vd = virtdisk.memory_virtdisk()
        End Sub

        Private Shared Function expected(ByVal p As UInt64) As Byte
            Return CByte(p And max_uint8)
        End Function

        Private Shared Sub rnd_write_data(ByRef start As UInt64, ByRef len As UInt32, ByRef buff() As Byte)
            start = CULng(rnd_int(0, 8192 * 8192 + 1))
            len = CUInt(rnd_int(1024, 1024 * 16 + 1))
            ReDim buff(CInt(len) - 1)
            For i As UInt32 = 0 To len - uint32_1
                buff(CInt(i)) = expected(start + i)
            Next
        End Sub

        Private Sub rnd_read_data(ByRef start As UInt64, ByRef len As UInt32)
            assertion.more(vd.size(), uint64_0)
            start = rnd_uint64(0, vd.size())
            len = rnd_uint(1, min(CUInt(vd.size() - start), CUInt(1024 * 16)) + uint32_1)
        End Sub

        Private Sub verify_data(ByVal start As UInt64, ByVal len As UInt32, ByVal buff() As Byte)
            assert(len > 0)
            assertion.equal(array_size(buff), len)
            assertion.more_or_equal(vd.size(), start + len)
            For i As Int32 = 0 To CInt(len) - 1
                assertion.is_true((buff(i) = expected(start + CUInt(i))) OrElse (buff(i) = 0))
            Next
        End Sub

        Public Overrides Function create() As event_comb
            Dim buff() As Byte = Nothing
            Dim start As UInt64 = 0
            Dim len As UInt32 = 0
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      rnd_write_data(start, len, buff)
                                      ec = vd.write(start, len, buff)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      rnd_read_data(start, len)
                                      ReDim buff(CInt(len) - 1)
                                      ec = vd.read(start, len, buff)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      verify_data(start, len, buff)
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
