
#If RETIRED
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.convertor

Public Class bytes_convertor_binder_initializer_test
    Inherits [case]

    Private Shared Function bind(Of T As Class)(ByRef f As T) As Boolean
        f = +(DirectCast(Nothing, binder(Of T, bytes_conversion_binder_protector)))
        Return assert_not_nothing(f)
    End Function

    Private Shared Function piece_case() As Boolean
        Dim p As piece = Nothing
        p = New piece(next_bytes(rnd_uint(1024, 4096)))
        '    Public Shared Function bind(ByVal f1 As _do_val_ref_ref(Of Byte(), UInt32, T, Boolean),
        '                                ByVal f2 As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, T, Boolean),
        '                                ByVal t1 As _do_val_val_ref(Of T, Byte(), UInt32, Boolean),
        '                                ByVal t2 As _do_val_ref(Of T, Byte(), Boolean)) As Boolean
        Dim f1 As _do_val_ref_ref(Of Byte(), UInt32, piece, Boolean) = Nothing
        If bind(f1) Then
            Dim o As piece = Nothing
            Dim offset As UInt32 = uint32_0
            offset = p.offset
            assert_true(f1(p.buff, offset, o))
            If assert_not_nothing(o) Then
                assert_equal(offset, p.offset + p.count)
                If assert_equal(p.count, o.count) Then
                    assert_equal(memcmp(p.buff, p.offset, o.buff, o.offset, o.count), 0)
                End If
            End If
        End If

        Dim f2 As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, piece, Boolean) = Nothing
        If bind(f2) Then
            Dim o As piece = Nothing
            assert_true(f2(p.buff, p.offset, p.count, o))
            assert_not_nothing(o)
            assert_equal(piece.compare(p, o), 0)
        End If

        Dim t1 As _do_val_val_ref(Of piece, Byte(), UInt32, Boolean) = Nothing
        If bind(t1) Then
            Dim b() As Byte = Nothing
            ReDim b((p.count << 1) - uint32_1)
            Dim i As UInt32 = uint32_0
            i = p.count
            assert_true(t1(p, b, i))
            assert_equal(i, p.count << 1)
            assert_equal(p.compare(b, p.count, p.count), 0)
        End If

        Dim t2 As _do_val_ref(Of piece, Byte(), Boolean) = Nothing
        If bind(t2) Then
            Dim b() As Byte = Nothing
            assert_true(t2(p, b))
            assert_equal(p.compare(b), 0)
        End If

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return piece_case()
    End Function
End Class
#End If
