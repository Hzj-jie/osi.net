
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class atomic_ref_T_test
    <test>
    Private Shared Sub can_exchange()
        Dim r As atomic_ref(Of String) = Nothing
        r = New atomic_ref(Of String)()
        assertion.is_null(r.exchange("abc"))
        assertion.reference_equal(r.exchange("def"), "abc")
        assertion.reference_equal(r.get(), "def")
    End Sub

    <test>
    Private Shared Sub can_compare_exchange()
        Dim r As atomic_ref(Of String) = Nothing
        r = New atomic_ref(Of String)()
        assertion.is_null(r.compare_exchange("abc", Nothing))
        assertion.reference_equal(r.compare_exchange("def", "abc"), "abc")
        assertion.reference_equal(r.get(), "def")
    End Sub

    Private Sub New()
    End Sub
End Class
