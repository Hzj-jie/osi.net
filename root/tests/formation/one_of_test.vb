
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class one_of_test
    <test>
    Public Sub first()
        Dim o As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(100)
        assertion.is_true(o.is_first())
        assertion.is_false(o.is_second())
        assertion.equal(o.first(), 100)
    End Sub

    <test>
    Public Sub second()
        Dim o As one_of(Of Int32, String) = one_of(Of Int32, String).of_second("abc")
        assertion.is_false(o.is_first())
        assertion.is_true(o.is_second())
        assertion.equal(o.second(), "abc")
    End Sub

    <test>
    Public Sub comparable()
        Dim o As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(100)
        Dim o2 As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(100)
        assertion.equal(o, o2)
    End Sub

    <test>
    Public Sub not_equal()
        Dim o As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(100)
        Dim o2 As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(101)
        assertion.not_equal(o, o2)
    End Sub

    <test>
    Public Sub not_equal_if_type_mismatch()
        Dim o As one_of(Of Int32, String) = one_of(Of Int32, String).of_first(100)
        Dim o2 As one_of(Of String, Int32) = one_of(Of String, Int32).of_second(100)
        assertion.not_equal(o.CompareTo(o2), 0)
    End Sub

    <test>
    Public Sub cloneable()
        Dim o As one_of(Of Int32, vector(Of Int32)) = one_of(Of Int32, vector(Of Int32)).of_second(vector.of(1))
        Dim o2 As one_of(Of Int32, vector(Of Int32)) = o.CloneT()
        assertion.is_false(o2.is_first())
        assertion.is_true(o2.is_second())
        assertion.not_reference_equal(o.second(), o2.second())
    End Sub

    Private Sub New()
    End Sub
End Class
