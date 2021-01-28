
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class hasharray_test2
    <test>
    Private Shared Sub should_shrink()
        Dim h As hasharray(Of Int32) = New hasharray(Of Int32)()
        For i As Int32 = 0 To 1000
            h.emplace(i)
        Next
        For i As Int32 = 0 To 1000
            assertion.equal(h.erase(i), uint32_1)
        Next
        assertion.is_true(h.shrink_to_fit())
    End Sub

    <test>
    Private Shared Sub should_not_shrink()
        Dim h As hasharray(Of String) = New hasharray(Of String)()
        For i As Int32 = 0 To 1000
            h.emplace(guid_str())
        Next
        assertion.is_false(h.shrink_to_fit())
    End Sub

    Private Sub New()
    End Sub
End Class
