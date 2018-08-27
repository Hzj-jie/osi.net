
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class ref_map_test
    <test>
    Private Shared Sub can_allocate_object()
        Dim m As ref_map(Of Int32, Int32) = Nothing
        m = New ref_map(Of Int32, Int32)()
        Dim r As ref_ptr(Of Int32) = Nothing
        r = m.get(100, Function() As Int32
                           Return 100
                       End Function)
        assert_true(r.referred())
        assert_equal(+r, 100)
        r.unref()
        r = m.get(100)
        assert_true(r.referred())
        assert_equal(+r, 100)
        r.unref()
    End Sub

    Private Sub New()
    End Sub
End Class
