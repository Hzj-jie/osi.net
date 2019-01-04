
Imports System.Diagnostics.CodeAnalysis
Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class wait_handle_behavior_test
    Inherits [case]

    Private Shared Function multiple_closes_in_multiple_threads(ByVal e As EventWaitHandle) As Boolean
        Const thread_count As Int32 = 100
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim are As AutoResetEvent = Nothing
        are = New AutoResetEvent(False)
        Dim finished As atomic_int = Nothing
        finished = New atomic_int()
        For i As Int32 = 0 To thread_count - 1
            queue_in_managed_threadpool(Sub()
                                            assert(mre.wait())
                                            e.Close()
                                            If finished.increment() = thread_count Then
                                                assert(are.force_set())
                                            End If
                                        End Sub)
        Next
        assert(mre.force_set())
        assert(are.wait())
        mre.Close()
        are.Close()
        Return True
    End Function

    Private Shared Function multiple_closes_in_multiple_threads() As Boolean
        For i As Int32 = 0 To 1000
            If Not multiple_closes_in_multiple_threads(New AutoResetEvent(False)) Then
                Return False
            End If

            If Not multiple_closes_in_multiple_threads(New ManualResetEvent(False)) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Shared Function multiple_closes(ByVal e As EventWaitHandle) As Boolean
        assert(Not e Is Nothing)
        For i As Int32 = 0 To 100
            e.Close()
        Next
        Return True
    End Function

    Private Shared Function multiple_sets_and_resets(ByVal e As EventWaitHandle) As Boolean
        assert(Not e Is Nothing)
        For i As Int32 = 0 To 100
            assertion.is_true(e.Set())
        Next
        For i As Int32 = 0 To 100
            assertion.is_true(e.Reset())
        Next
        Return True
    End Function

    <SuppressMessage("", "BC40000")>
    Private Shared Function a_valid_handle_after_close(ByVal e As WaitHandle) As Boolean
        assert(Not e Is Nothing)
        assertion.is_not_null(e.SafeWaitHandle())
        assertion.is_false(e.Handle() = IntPtr.Zero)
        e.Close()
        assertion.is_not_null(e.SafeWaitHandle())
        assertion.is_false(e.Handle() = IntPtr.Zero)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return a_valid_handle_after_close(New AutoResetEvent(False)) AndAlso
               a_valid_handle_after_close(New ManualResetEvent(False)) AndAlso
               multiple_sets_and_resets(New AutoResetEvent(False)) AndAlso
               multiple_sets_and_resets(New ManualResetEvent(False)) AndAlso
               multiple_closes(New AutoResetEvent(False)) AndAlso
               multiple_closes(New ManualResetEvent(False)) AndAlso
               multiple_closes_in_multiple_threads()
    End Function
End Class
