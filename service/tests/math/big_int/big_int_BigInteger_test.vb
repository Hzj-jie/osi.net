
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_int_BigInteger_test
    <repeat(100000)>
    <test>
    Private Shared Sub add()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random()
        b = big_int.random()
        assertion.equal(a + b, big_int.from_BigInteger(a.as_BigInteger() + b.as_BigInteger()))
    End Sub

    <repeat(100000)>
    <test>
    Private Shared Sub [sub]()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random()
        b = big_int.random()
        assertion.equal(a - b, big_int.from_BigInteger(a.as_BigInteger() - b.as_BigInteger()))
    End Sub

    <repeat(100000)>
    <test>
    Private Shared Sub multiply()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random()
        b = big_int.random()
        assertion.equal(a * b, big_int.from_BigInteger(a.as_BigInteger() * b.as_BigInteger()))
    End Sub

    <repeat(100000)>
    <test>
    Private Shared Sub divide()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random()
        b = big_int.random()
        While b.is_zero()
            b = big_int.random()
        End While
        assertion.equal(a \ b, big_int.from_BigInteger(a.as_BigInteger() / b.as_BigInteger()))
    End Sub

    <command_line_specified>
    <repeat(10000000)>
    <test>
    Private Shared Sub divide_large()
        divide()
    End Sub

    <repeat(100000)>
    <test>
    Private Shared Sub modulus()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random()
        b = big_int.random()
        While b.is_zero()
            b = big_int.random()
        End While
        assertion.equal(a Mod b, big_int.from_BigInteger(a.as_BigInteger() Mod b.as_BigInteger()))
    End Sub

    <command_line_specified>
    <repeat(10000000)>
    <test>
    Private Shared Sub modulus_large()
        modulus()
    End Sub

    <repeat(100)>
    <test>
    Private Shared Sub power()
        Dim a As big_int = Nothing
        Dim b As Int32 = 0
        a = big_int.random()
        b = rnd_int(1, 127)
        assertion.equal(a ^ b, big_int.from_BigInteger(BigInteger.Pow(a.as_BigInteger(), b)))
    End Sub

    <repeat(100000)>
    <test>
    Private Shared Sub cast()
        Dim a As big_int = Nothing
        a = big_int.random()
        assertion.equal(a, big_int.from_BigInteger(a.as_BigInteger()))
    End Sub

    <repeat(10000)>
    <test>
    Private Shared Sub gcd()
        Dim a As big_int = Nothing
        Dim b As big_int = Nothing
        a = big_int.random(20)
        b = big_int.random(20)
        For i As Int32 = 0 To 20
            Dim c As big_int = Nothing
            c = big_int.random(20)
            If rnd_bool() Then
                a.multiply(c)
            End If
            If rnd_bool() Then
                b.multiply(c)
            End If
        Next
        a.set_positive()
        b.set_positive()
        assertion.equal(New big_int(big_uint.gcd(a.abs_big_uint(), b.abs_big_uint())),
                        big_int.from_BigInteger(BigInteger.GreatestCommonDivisor(a.as_BigInteger(), b.as_BigInteger())))
    End Sub

    Private Shared Sub cast_predefined_case(ByVal i As Int32)
        Dim a As big_int = Nothing
        a = New big_int(i)
        assertion.equal(a, big_int.from_BigInteger(a.as_BigInteger()))
    End Sub

    <test>
    Private Shared Sub cast_predefined()
        cast_predefined_case(1)
        cast_predefined_case(-1)
        cast_predefined_case(0)
    End Sub

    Private Sub New()
    End Sub
End Class
