
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class piece_test
    Private Shared Function random_piece() As piece
        If rnd_bool_trues(6) Then
            Return piece.blank
        Else
            Dim b() As Byte = Nothing
            b = next_bytes(rnd_uint(1024, 4096))
            Dim s As UInt32 = 0
            s = rnd_uint(0, array_size(b))
            Dim l As UInt32 = 0
            l = rnd_uint(0, array_size(b) - s)
            Return New piece(b, s, l)
        End If
    End Function

    Private Shared Function random_serialized_piece(ByRef v As vector(Of piece)) As piece
        v.renew()
        For i As Int32 = 0 To rnd_int(1, 10) - 1
            v.emplace_back(random_piece())
        Next
        Dim r As piece = Nothing
        assert_true(bytes_serializer(Of vector(Of piece)).default.to_bytes(v, r))
        Return r
    End Function

    Private Shared Sub empty_to_null(ByVal i As vector(Of piece))
        assert(Not i Is Nothing)
        Dim j As UInt32 = 0
        While j < i.size()
            If i(j).null_or_empty() Then
                i(j) = Nothing
            End If
            j += uint32_1
        End While
    End Sub

    <test>
    <repeat(500, 50000)>
    Private Shared Sub serializable()
        Dim original As vector(Of piece) = Nothing
        Dim p As piece = Nothing
        p = random_serialized_piece(original)

        Dim v As vector(Of piece) = Nothing
        assert_true(bytes_serializer(Of vector(Of piece)).default.from_bytes(p, v))
        assert_equal(v, original)

        ' TODO: Should compare support delegating null to the implementation?
        empty_to_null(original)

        Dim bytes As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer(Of vector(Of Byte())).default.from_bytes(p, bytes))
        assert_true(deep_compare(+bytes, +original) = 0)

        Dim serialized() As Byte = Nothing
        assert_true(bytes_serializer(Of vector(Of Byte())).default.to_bytes(bytes, serialized))
        assert_true(compare(p, serialized) = 0)
    End Sub

    <test>
    Private Shared Sub null_serializable()
        Dim p As piece = Nothing
        Dim r() As Byte = Nothing
        assert_true(bytes_serializer.to_bytes(p, r))
        assert_true(bytes_serializer.from_bytes(r, p))
        assert_not_nothing(p)
        assert_true(p.empty())
    End Sub

    Private Sub New()
    End Sub
End Class
