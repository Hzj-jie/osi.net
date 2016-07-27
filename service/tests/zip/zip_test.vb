
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.zip

Public Class zip_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New zip_case(), 1024)
    End Sub

    Private Class zip_case
        Inherits [case]

        Private Shared Function run_case(ByVal parameter As String) As Boolean
            Dim zipper As zipper = Nothing
            If assert_true(osi.service.zip.create(parameter, zipper)) AndAlso
               assert_not_nothing(zipper) AndAlso
               assert_true(Not zipper.bypass() OrElse
                           strsame(parameter, bypass_mode)) Then
                Dim raw() As Byte = Nothing
                raw = rndbytes(rnd_int(4096, 65536))
                assert(Not isemptyarray(raw))
                Dim zipped() As Byte = Nothing
                Dim unzipped() As Byte = Nothing
                If assert_true(zipper.zip(raw, zipped)) AndAlso
                   assert_true(zipper.unzip(zipped, unzipped)) Then
                    assert_array_equal(raw, unzipped)
                    If zipper.bypass() OrElse zipper.bypass2() Then
                        assert_array_equal(zipped, raw)
                    Else
                        assert_array_not_equal(zipped, raw)
                    End If
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return run_case("bypass") AndAlso
                   run_case("bypass2") AndAlso
                   run_case("deflate") AndAlso
                   run_case("gzip")
        End Function
    End Class
End Class
