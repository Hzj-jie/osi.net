
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.formation
Imports osi.service.http

Public Class http_client_test
    Inherits [case]

    Private Shared ReadOnly headers(,) As String = {{"header1", "value1.0"},
                                                    {"header1", "value1.1"},
                                                    {"header2", "value2"},
                                                    {"header3", Nothing},
                                                    {"header3", "value3"},
                                                    {"header4", "value4"}}

    Private Shared Sub assert_result_size(ByVal headers(,) As String, ByVal m As map(Of String, vector(Of String)))
        assert(Not m Is Nothing)
        Dim c As UInt32 = 0
        Dim it As map(Of String, vector(Of String)).iterator = Nothing
        it = m.begin()
        While it <> m.end()
            c += (+it).second.size()
            it += 1
        End While
        assertion.equal(c, array_size(headers))
    End Sub

    Private Shared Sub assert_coverage(ByVal headers(,) As String, ByVal m As map(Of String, vector(Of String)))
        assert(Not m Is Nothing)
        For i As Int32 = 0 To array_size(headers) - 1
            Dim it As map(Of String, vector(Of String)).iterator = Nothing
            it = m.find(headers(i, 0))
            assertion.is_true(it <> m.end())
            assertion.is_false(String.IsNullOrEmpty((+it).first))
            assertion.is_false((+it).second.empty())
            assertion.is_true((+((+it).second)).has(headers(i, 1)))
        Next
    End Sub

    Private Shared Function run_case(ByVal headers(,) As String) As Boolean
        Dim m As map(Of String, vector(Of String)) = Nothing
        assertion.is_true(headers.to_http_headers(m))
        assert_result_size(headers, m)
        assert_coverage(headers, m)
        Return True
    End Function

    Private Shared Function static_case() As Boolean
        Return run_case(headers)
    End Function

    Private Shared Function random_key() As String
        Return rnd_en_chars(rnd_int(1, 10))
    End Function

    Private Shared Function random_value() As String
        Return rnd_en_chars(rnd_int(0, 10))
    End Function

    Private Shared Function random_headers() As String(,)
        Dim r(,) As String = Nothing
        ReDim r(rnd_int(5, 20), 1)
        For i As Int32 = 0 To array_size(r) - 1
            r(i, 0) = random_key()
            r(i, 1) = random_value()
        Next
        Return r
    End Function

    Private Shared Function random_case() As Boolean
        For i As Int32 = 0 To 1024 * 32 - 1
            If Not run_case(random_headers()) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function failure_case() As Boolean
        Dim r(,) As String = Nothing
        Dim m As map(Of String, vector(Of String)) = Nothing
        assertion.is_false(r.to_http_headers(m))
        ReDim r(10, 2)
        assertion.is_false(r.to_http_headers(m))
        ReDim r(10, 1)
        assertion.is_false(r.to_http_headers(m))
        For i As Int32 = 0 To array_size(r) - 1
            r(i, 0) = empty_string
        Next
        assertion.is_false(r.to_http_headers(m))
        For i As Int32 = 0 To array_size(r) - 1
            r(i, 0) = random_key()
        Next
        r(rnd_int(0, array_size(r) - 1), 0) = empty_string
        assertion.is_false(r.to_http_headers(m))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return static_case() AndAlso
               random_case() AndAlso
               failure_case()
    End Function
End Class
