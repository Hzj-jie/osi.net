
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class shuffle_test
    <test>
    Private Shared Sub primitive_type()
        Dim v() As Int32 = Nothing
        v = rnd_ints(100)
        Dim o() As Int32 = Nothing
        o = v.shuffle()
        assertion.not_reference_equal(v, o)
        assertion.array_not_equal(v, o)
    End Sub

    <test>
    Private Shared Sub reference_type()
        Dim v() As String = Nothing
        ReDim v(100)
        For i As Int32 = 0 To array_size_i(v) - 1
            v(i) = rnd_ascii_display_chars(20)
        Next
        Dim o() As String = Nothing
        o = v.shuffle()
        assertion.not_reference_equal(v, o)
        assertion.array_not_equal(v, o)
    End Sub

    Private Sub New()
    End Sub
End Class
