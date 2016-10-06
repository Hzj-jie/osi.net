
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class lcs_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New lcs_case(), 1024), 2)
    End Sub

    Private Class lcs_case
        Inherits [case]

        Private Shared Function stupid_lcs(ByVal this As String, ByVal that As String) As String
            Dim r As String = Nothing
            For i As UInt32 = 0 To strlen(this) - 1
                For j As UInt32 = 1 To strlen(this) - i
                    Dim s As String = Nothing
                    s = strmid(this, i, j)
                    If strcontains(that, s) AndAlso j > strlen(r) Then
                        r = s
                    End If
                Next
            Next
            Return r
        End Function

        Public Overrides Function run() As Boolean
            Dim a As String = Nothing
            Dim b As String = Nothing
            a = rnd_chars(rnd_int(max_uint8, CInt(max_uint8) << 1))
            b = rnd_chars(rnd_int(max_uint8, CInt(max_uint8) << 1))
            Dim s1 As String = Nothing
            s1 = lcs(a, b)
            Dim s2 As String = Nothing
            s2 = stupid_lcs(a, b)
            assert_equal(s1, s2)
            Return True
        End Function
    End Class
End Class
