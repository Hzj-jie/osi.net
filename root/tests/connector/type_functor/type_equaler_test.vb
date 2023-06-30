
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_equaler_test
    <test>
    <repeat(10000)>
    Private Shared Sub int_case()
        Dim x As Int32 = 0
        Dim y As Int32 = 0
        x = rnd_int()
        y = rnd_int()
        assertion.equal(equal(x, y), type_equaler.equal(GetType(Int32), GetType(Int32), x, y))
        assertion.equal(type_equaler.equal(GetType(Int32), GetType(Int32), x, x), True)
        assertion.equal(type_equaler.equal(GetType(Int32), GetType(Int32), y, y), True)

        assertion.equal(equal(x, y), type_equaler.infer_equal(x, y))
        assertion.equal(type_equaler.infer_equal(x, x), True)
        assertion.equal(type_equaler.infer_equal(y, y), True)
    End Sub

    <test>
    <repeat(10000)>
    Private Shared Sub string_case()
        Dim x As String = Nothing
        Dim y As String = Nothing
        x = rnd_utf8_chars(rnd_int(10, 1000))
        y = rnd_utf8_chars(rnd_int(10, 1000))
        assertion.equal(equal(x, y), type_equaler.equal(GetType(String), GetType(String), x, y))
        assertion.equal(type_equaler.equal(GetType(String), GetType(String), x, x), True)
        assertion.equal(type_equaler.equal(GetType(String), GetType(String), y, y), True)

        assertion.equal(equal(x, y), type_equaler.infer_equal(x, y))
        assertion.equal(type_equaler.infer_equal(x, x), True)
        assertion.equal(type_equaler.infer_equal(y, y), True)
    End Sub

    Private Sub New()
    End Sub
End Class