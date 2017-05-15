
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
        p = make_weak_ref_pointer(New cd_object(Of weak_ref_pointer_test)())
        For i As Int32 = 0 To 1000
            p.set(New cd_object(Of weak_ref_pointer_test)())
            repeat_gc_collect()
        Next
        assert_more(cd_object(Of weak_ref_pointer_test).destructed(), uint32_0)
        Return True
    End Function

    Private Shared Function event_raised() As Boolean
        Const size As Int32 = 1000
        Dim ps() As weak_ref_pointer(Of Int32) = Nothing
        ReDim ps(size - 1)
        Dim c As Int32 = 0
        For i As Int32 = 0 To size - 1
            ps(i) = make_weak_ref_pointer(i)
            Dim ci As Int32 = 0
            ci = i
            AddHandler ps(i).finalized, Sub(j As Int32)
                                            assert_false(ps(ci).alive())
                                            assert_equal(ci, j)
                                            c += 1
                                        End Sub
            repeat_gc_collect()
        Next
        assert_more(c, 0)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return wont_pin_objects() AndAlso
               event_raised()
    End Function
End Class
