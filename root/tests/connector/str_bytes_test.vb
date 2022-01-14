
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class str_bytes_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(chained(repeat(New str_bytes_case(), 1024 * 1024), New str_bytes_specific_cases()))
    End Sub

    Private Class str_bytes_specific_cases
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim o() As Byte = Nothing
            assertion.is_false(str_bytes_ref(Nothing, o))
            assertion.is_true(str_bytes_ref("", o))
            assertion.equal(array_size(o), uint32_0)
            assertion.is_null(str_bytes(Nothing))
            assertion.is_not_null(str_bytes(""))
            Return True
        End Function
    End Class

    Private Class str_bytes_case
        Inherits [case]

        Private Shared Function run_case(ByVal s As String) As Boolean
            Dim b() As Byte = Nothing
            b = str_bytes(s)
            assertion.equal(str_byte_count(s), array_size(b))
            assertion.is_not_null(b)
            If String.IsNullOrEmpty(s) Then
                assertion.is_null(bytes_str(b))
            Else
                assertion.equal(bytes_str(b), s)
            End If

            Dim i As Int32 = 0
            i = rnd_int(1, strlen(s) + 1)
            If i = strlen(s) Then
                assertion.is_not_null(bytes_str(str_bytes(s, i)))
                assertion.is_true(String.IsNullOrEmpty(bytes_str(str_bytes(s, i))))
                assertion.equal(bytes_str(str_bytes(s), str_byte_count(s)), "")
            Else
                assertion.equal(bytes_str(str_bytes(s, i)), strmid(s, i))
                assertion.equal(bytes_str(str_bytes(s), str_byte_count(strleft(s, i))), strmid(s, i))
            End If

            Dim j As Int32 = 0
            j = rnd_int(i, strlen(s) + 1)
            If i = j Then
                assertion.is_not_null(bytes_str(str_bytes(s, i, uint32_0)))
                assertion.is_true(String.IsNullOrEmpty(bytes_str(str_bytes(s, i, uint32_0))))
                assertion.equal(bytes_str(str_bytes(s), str_byte_count(strleft(s, i)), uint32_0), "")
            Else
                assertion.equal(bytes_str(str_bytes(s, i, j - i)), strmid(s, i, j - i))
                assertion.equal(bytes_str(str_bytes(s),
                                       str_byte_count(strleft(s, i)),
                                       str_byte_count(s) -
                                       str_byte_count(strleft(s, i)) -
                                       str_byte_count(strmid(s, j))),
                             strmid(s, i, j - i))
            End If

            Return True
        End Function

        Public Overrides Function run() As Boolean
            'str_bytes uses utf8 encoder.
            Return run_case(rnd_utf8_chars(rnd_int(16, 1024)))
        End Function
    End Class
End Class
