
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class weak_ref_pointer_test
    Inherits [case]

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            cd_object(Of weak_ref_pointer_test).reset()
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function wont_pin_objects() As Boolean
        Dim p As weak_ref_pointer(Of cd_object(Of weak_ref_pointer_test)) = Nothing
        p = weak_ref_pointer.of(New cd_object(Of weak_ref_pointer_test)())
        For i As Int32 = 0 To 1000
            p.set(New cd_object(Of weak_ref_pointer_test)())
            garbage_collector.repeat_collect()
        Next
        assertion.more(cd_object(Of weak_ref_pointer_test).destructed(), uint32_0)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return wont_pin_objects()
    End Function
End Class
