
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_str_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_uint_str_case(), round_scale * 32 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function run_case(ByVal u As Int32, ByVal b As Byte) As Boolean
        Dim s As String = Nothing
        s = big_uint.rnd_support_str(rnd_int(1, u), b)
        Dim r As big_uint = Nothing
        assert_true(big_uint.parse(s, r, b))
        assert_equal(r.str(b), s)
        Return True
    End Function

    Private Shared Function fast_rnd_case() As Boolean
        For b As Byte = 0 To max_uint8 - 1
            If big_uint.support_base(b) Then
                If Not run_case(30, b) Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function

    Private Shared Function rnd_case() As Boolean
        Return run_case(1000, big_uint.rnd_support_base())
    End Function

    Private Shared Function failure_case() As Boolean
        Dim b As Byte = 0
        b = big_uint.rnd_support_base()
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        Dim l As Int32 = 0
        l = rnd_int(0, 1000)
        For i As Int32 = 0 To l - 1
            s.Append(rnd_ascii_display_char())
        Next
        'make sure there is at least one unsupported character
        s.Append(big_uint.rnd_unsupport_str_char(b))
        assert_false(big_uint.parse(Convert.ToString(s), Nothing, b))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return fast_rnd_case() AndAlso
               rnd_case() AndAlso
               failure_case()
    End Function
End Class
