
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class ref_ptr_test2
    <test>
    Private Shared Sub should_release_object_in_finalize()
        For i As Int32 = 0 To 10000
            Dim r As ref_ptr(Of cd_object(Of joint_type(Of ref_ptr_test2, _0))) = Nothing
            r = ref_ptr.[New](New cd_object(Of joint_type(Of ref_ptr_test2, _0))())
            assertion.equal(cd_object(Of joint_type(Of ref_ptr_test2, _0)).constructed(), CUInt(i + 1))
            r = Nothing
            garbage_collector.waitfor_collect()
            If cd_object(Of joint_type(Of ref_ptr_test2, _0)).disposed() > 0 Then
                Return
            End If
        Next
        garbage_collector.waitfor_collect()

        assertion.more(cd_object(Of joint_type(Of ref_ptr_test2, _0)).disposed(), uint32_0)
    End Sub

    Private Sub New()
    End Sub
End Class
