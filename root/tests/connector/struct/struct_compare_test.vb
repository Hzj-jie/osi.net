
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class struct_compare_test
    Private NotInheritable Class c
        Public ReadOnly a As String
        Public ReadOnly b As vector(Of Int32)
        Public ReadOnly c As Boolean
        Public ReadOnly d As Byte
        Public ReadOnly e As UInt64

        Shared Sub New()
            struct(Of c).register()
        End Sub

        Private Sub New(ByVal a As String,
                        ByVal b As vector(Of Int32),
                        ByVal c As Boolean,
                        ByVal d As Byte,
                        ByVal e As UInt64)
            Me.a = a
            Me.b = b
            Me.c = c
            Me.d = d
            Me.e = e
        End Sub

        Public Sub New()
            Me.New(rnd_utf8_chars(rnd_int(16, 32)),
                   vector.emplace_of(rnd_ints(rnd_int(16, 32))),
                   rnd_bool(),
                   rnd_byte(),
                   rnd_uint64())
        End Sub

        Public Function clone() As c
            Return New c(copy(a), copy(b), copy(c), copy(d), copy(e))
        End Function

        Public Function compare_to(ByVal r As c) As Int32
            assert(r IsNot Nothing)
            Dim c As Int32 = 0
            c = compare(a, r.a)
            If c <> 0 Then
                Return c
            End If
            c = compare(b, r.b)
            If c <> 0 Then
                Return c
            End If
            c = compare(Me.c, r.c)
            If c <> 0 Then
                Return c
            End If
            c = compare(d, r.d)
            If c <> 0 Then
                Return c
            End If
            c = compare(e, r.e)
            If c <> 0 Then
                Return c
            End If
            Return 0
        End Function
    End Class

    <test>
    <repeat(100000)>
    Private Shared Sub run()
        Dim a As c = Nothing
        a = New c()
        Dim b As c = Nothing
        b = New c()
        assertion.equal(compare(a, b), a.compare_to(b))
        assertion.equal(equal(a, b), a.compare_to(b) = 0)
        assertion.equal(compare(a, a.clone()), 0)
        assertion.is_true(equal(a, a.clone()))
        assertion.equal(compare(b, b.clone()), 0)
        assertion.is_true(equal(b, b.clone()))
    End Sub

    Private Sub New()
    End Sub
End Class
