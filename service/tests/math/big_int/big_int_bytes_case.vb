
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Friend Class big_int_bytes_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_int_bytes_case(), round_scale * 102400 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function run() As Boolean
        Dim r As Int64 = 0
        r = rnd_int64(min_int64 + int64_1, max_int64)
        Dim b1 As big_int = Nothing
        If r >= 0 Then
            b1 = New big_int(array_concat({byte_0}, int64_bytes(r)))
        Else
            b1 = New big_int(array_concat({byte_1}, int64_bytes(-r)))
        End If
        Dim b2 As big_int = Nothing
        b2 = New big_int(r)
        assertion.equal(b1, b2)

        b1 = big_int.random()
        b2 = New big_int(b1.as_bytes())
        assertion.equal(b1, b2)
        Return True
    End Function
End Class
