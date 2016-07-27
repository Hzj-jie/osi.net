
Imports System.Threading
Imports osi.root.connector
Imports osi.root.utt

Public Class wait_handle_behavior_test
    Inherits [case]

    Private Shared Function multiple_sets_and_resets(ByVal e As EventWaitHandle) As Boolean
        assert(Not e Is Nothing)
        For i As Int32 = 0 To 100
            assert_true(e.Set())
        Next
        For i As Int32 = 0 To 100
            assert_true(e.Reset())
        Next
        Return True
    End Function

    Private Shared Function a_valid_handle_after_close(ByVal e As WaitHandle) As Boolean
        assert(Not e Is Nothing)
        assert_not_nothing(e.SafeWaitHandle())
        assert_false(e.Handle() = IntPtr.Zero)
        e.Close()
        assert_not_nothing(e.SafeWaitHandle())
        assert_false(e.Handle() = IntPtr.Zero)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return a_valid_handle_after_close(New AutoResetEvent(False)) AndAlso
               a_valid_handle_after_close(New ManualResetEvent(False)) AndAlso
               multiple_sets_and_resets(New AutoResetEvent(False)) AndAlso
               multiple_sets_and_resets(New ManualResetEvent(False))
    End Function
End Class
