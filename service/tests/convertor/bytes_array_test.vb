
#If RETIRED
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.convertor

Public Class bytes_array_test
    Inherits [case]

    Private Shared Function to_bytes_case() As Boolean
        Dim c As vector(Of Byte()) = Nothing
        c = New vector(Of Byte())()
        For i As Int32 = 0 To rnd_int(20, 100) - 1
            c.emplace_back(next_bytes(rnd_int(0, 256)))
        Next
        Dim b() As Byte = Nothing
        b = c.to_bytes()
        If assert_false(isemptyarray(b)) Then
            Dim r As vector(Of Byte()) = Nothing
            r = b.to_vector_bytes()
            If assert_not_nothing(r) Then
                assert_equal(r.size(), c.size())
                For i As Int32 = 0 To r.size() - 1
                    assert_true(memcmp(r(i), c(i)) = 0)
                Next
            End If
        End If
        Return True
    End Function

    Private Shared Function to_chunk_case() As Boolean
        Dim c As vector(Of Byte()) = Nothing
        c = New vector(Of Byte())()
        For i As Int32 = 0 To rnd_int(20, 100) - 1
            c.push_back(next_bytes(rnd_int(0, 256)))
        Next
        Dim b() As Byte = Nothing
        For i As Int32 = 0 To c.size() - 1
            b.append(c(i).to_chunk())
        Next
        If assert_false(isemptyarray(b)) Then
            Dim r As vector(Of Byte()) = Nothing
            r = b.to_vector_bytes()
            If assert_not_nothing(r) Then
                assert_equal(r.size(), c.size())
                For i As Int32 = 0 To r.size() - 1
                    assert_true(memcmp(r(i), c(i)) = 0)
                Next
            End If
        End If
        Return True
    End Function

    Private Shared Function to_chunk_consistence_case() As Boolean
        For i As Int32 = 0 To 1024 - 1
            Dim b() As Byte = Nothing
            ReDim b(rnd_int(1, 1024) - 1)
            Dim l As UInt32 = 0
            assert_true(parse_as_preamble(to_chunk(b), l))
            assert_equal(l, array_size(b))
        Next
        Return True
    End Function

    Private Shared Function run_case() As Boolean
        Return to_bytes_case() AndAlso
               to_chunk_case() AndAlso
               to_chunk_consistence_case()
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 32 - 1
            If Not run_case() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
#End If
