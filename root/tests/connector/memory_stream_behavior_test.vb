
Imports System.IO
Imports osi.root.connector
Imports osi.root.utt

Public Class memory_stream_behavior_test
    Inherits [case]

    Private Shared Function write_after_shrink() As Boolean
        Dim ms As MemoryStream = Nothing
        ms = New MemoryStream()
        Dim buff() As Byte = Nothing
        buff = rnd_bytes()
        ms.Write(buff, 0, array_size(buff))
        assertion.equal(ms.Position(), array_size(buff))
        Dim buff2() As Byte = Nothing
        buff2 = ms.fit_buffer()
        assertion.array_equal(buff, buff2)
        ms.Write(buff, 0, array_size(buff))
        assertion.equal(ms.Position(), array_size(buff) << 1)
        buff2 = ms.fit_buffer()
        buff.append(buff)
        assertion.array_equal(buff, buff2)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return write_after_shrink()
    End Function
End Class
