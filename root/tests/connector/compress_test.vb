
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt

Public Class compress_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New compress_case(), 1024)
    End Sub

    Private Class compress_case
        Inherits [case]

        Private Shared Function run_case(ByVal compress As _do_val_ref(Of Byte(), Byte(), Boolean),
                                         ByVal decompress As _do_val_ref(Of Byte(), Byte(), Boolean)) As Boolean
            assert(Not compress Is Nothing)
            assert(Not decompress Is Nothing)
            Dim unzipped() As Byte = Nothing
            Dim zipped() As Byte = Nothing
            unzipped = rndbytes(rnd_int(1024, 65536))
            If assert_true(compress(unzipped, zipped)) Then
                Dim zipunzipped() As Byte = Nothing
                If assert_true(decompress(zipped, zipunzipped)) Then
                    assert_array_equal(unzipped, zipunzipped)
                    assert_array_not_equal(zipped, unzipped)
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return run_case(AddressOf gzip, AddressOf ungzip) AndAlso
                   run_case(AddressOf deflate, AddressOf undeflate)
        End Function
    End Class
End Class
