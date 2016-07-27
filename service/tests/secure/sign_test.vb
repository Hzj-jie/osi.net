
Imports System.Security.Cryptography
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utt
Imports osi.service.secure
Imports osi.service.secure.sign

Public Class sign_test
    Inherits chained_case_wrapper

    Private Shared Function c(ByVal ParamArray s() As signer) As [case]()
        assert(Not isemptyarray(s))
        Dim r() As [case] = Nothing
        ReDim r(array_size(s) - uint32_1)
        For i As UInt32 = uint32_0 To array_size(s) - uint32_1
            r(i) = repeat(New sign_case(s(i)), 1000)
        Next
        Return r
    End Function

    Public Sub New()
        MyBase.New(c(sign.all_signers()))
    End Sub

    Private Class sign_case
        Inherits [case]

        Private ReadOnly s As signer

        Public Sub New(ByVal s As signer)
            assert(Not s Is Nothing)
            Me.s = s
        End Sub

        Public Overrides Function run() As Boolean
            Dim k() As Byte = Nothing
            Dim b() As Byte = Nothing
            Dim o() As Byte = Nothing
            Dim o2() As Byte = Nothing
            k = rndbytes(rnd_uint(100, 200))
            b = rndbytes(rnd_uint(2048, 4096))
            assert_true(s.sign(k, b, o))
            assert_true(s.sign(k, b, o2))
            assert_array_equal(o, o2)
            Dim l As UInt32 = uint32_0
            Dim r As UInt32 = uint32_0
            l = rnd_uint(10, 100)
            r = rnd_uint(500, 1000)
            assert_true(s.sign(k, b, l, r, o))
            assert_true(s.sign(k, b, l, r, o2))
            assert_array_equal(o, o2)

            For i As Int32 = 0 To 3
                Dim b2() As Byte = Nothing
                b2 = rndbytes(rnd_uint(2048, 4096))
                assert_true(s.sign(k, b, o))
                assert_true(s.sign(k, b2, o2))
                If memcmp(o, o2) <> 0 Then
                    Return True
                End If
            Next
            assert_true(False)
            Return True
        End Function
    End Class
End Class
