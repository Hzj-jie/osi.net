
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class for_each_behavior_test
    <test>
    Private Shared Sub against_null_array()
        assertion.thrown(Sub()
                             Dim vs() As Byte = Nothing
                             For Each v As Byte In vs
                             Next
                         End Sub)
    End Sub

    <test>
    Private Shared Sub against_empty_array()
        Dim vs(-1) As Byte
        For Each v As Byte In vs
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
