
Imports osi.root.connector
Imports osi.root.utt

Public Class bytes_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New perf_case(), 32 * 1024 * 1024))
    End Sub

    Private Class perf_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As Int64 = 0
            x = rnd_int64()
            x.big_endian_bytes().as_big_endian_int64()
            x.little_endian_bytes().as_little_endian_int64()
            x.bytes().as_int64()
            Return True
        End Function
    End Class
End Class
