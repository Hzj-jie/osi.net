
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.secure
Imports osi.service.secure.sign

Public Class sign_strongness_test
    Inherits chained_case_wrapper

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 1000)
    End Function

    Private Shared Function oc(ByVal s As signer) As [case]()
        Return {r(New sign_strongness_case(s)),
                r(New sign_strongness_case2(s)),
                r(New sign_strongness_case3(s))}
    End Function

    Private Shared Function c(ByVal ParamArray s() As signer) As [case]()
        assert(Not isemptyarray(s))
        Dim r() As [case] = Nothing
        For i As Int32 = 0 To array_size_i(s) - 1
            r = array_concat(r, oc(s(i)))
        Next
        Return r
    End Function

    Public Sub New()
        MyBase.New(True, c(sign.all_signers()))
    End Sub

    Private Class sign_strongness_case
        Inherits [case]

        Private ReadOnly s As signer
        Private ReadOnly rs As [set](Of piece)

        Public Sub New(ByVal s As signer)
            assert(Not s Is Nothing)
            Me.s = s
            Me.rs = New [set](Of piece)()
        End Sub

        Protected Overridable Function key() As Byte()
            Return rnd_bytes(rnd_uint(100, 200))
        End Function

        Protected Overridable Function data() As Byte()
            Return rnd_bytes(rnd_uint(1000, 2000))
        End Function

        Public NotOverridable Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                rs.clear()
                Return True
            Else
                Return False
            End If
        End Function

        Public NotOverridable Overrides Function run() As Boolean
            Dim k() As Byte = Nothing
            Dim b() As Byte = Nothing
            k = key()
            b = data()
            assert(Not isemptyarray(k))
            assert(Not isemptyarray(b))
            Dim r() As Byte = Nothing
            assert_true(s.sign(k, b, r))
            If Not assert_true(rs.insert(r).second,
                               lazier.[New](Function() strcat("signer ", s.GetType(), ", case ", Me.GetType()))) Then
                Return False
            End If
            Return True
        End Function

        Public NotOverridable Overrides Function finish() As Boolean
            rs.clear()
            Return MyBase.finish()
        End Function
    End Class

    Private Class sign_strongness_case2
        Inherits sign_strongness_case

        Private ReadOnly k() As Byte

        Public Sub New(ByVal s As signer)
            MyBase.New(s)
            k = MyBase.key()
        End Sub

        Protected Overrides Function key() As Byte()
            Return k
        End Function
    End Class

    Private Class sign_strongness_case3
        Inherits sign_strongness_case

        Private Shared ReadOnly weak_signer() As signer = {
            bypass.instance,
            bypass2.instance,
            ring.instance,
            md5_merge_hasher.ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.md5_new).ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.ripemd160_new).ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.sha1_new).ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.sha256_new).ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.sha384_new).ring,
            hash_algorithm_merge_hasher(Of hash_algorithm_merge_hasher.sha512_new).ring}
        Private ReadOnly expected_weak As Boolean
        Private ReadOnly b() As Byte

        Public Sub New(ByVal s As signer)
            MyBase.New(s)
            expected_weak = weak_signer.has(s)
            If Not expected_weak Then
                b = MyBase.data()
            End If
        End Sub

        Protected Overrides Function data() As Byte()
            If expected_weak Then
                Return MyBase.data()
            Else
                Return b
            End If
        End Function
    End Class
End Class
