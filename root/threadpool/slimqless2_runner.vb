﻿
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock

Public Class slimqless2_runner
    Private ReadOnly q As waitable_slimqless2(Of Action)
    Private ReadOnly t As Thread
    Private s As singleentry

    Public Sub New()
        Me.New(New waitable_slimqless2(Of Action)())
    End Sub

    Public Sub New(ByVal q As waitable_slimqless2(Of Action))
        assert(Not q Is Nothing)
        Me.q = q
        Me.t = New Thread(Sub()
                              While Not stopping()
                                  If Not execute() Then
                                      assert(wait(max_int64) OrElse stopping())
                                  End If
                              End While
                          End Sub)
        t.Start()
        assert(Not stopping())
    End Sub

    Public Function stopping() As Boolean
        Return s.in_use()
    End Function

    Public Sub [stop]()
        If s.mark_in_use() Then
            t.Abort()
            GC.SuppressFinalize(Me)
        End If
    End Sub

    Public Function join(ByVal timeout_ms As Int64) As Boolean
        If timeout_ms > max_int32 Then
            Return t.Join(max_int32)
        ElseIf timeout_ms < 0 Then
            Return t.Join(Timeout.Infinite)
        Else
            Return t.Join(CInt(timeout_ms))
        End If
    End Function

    Public Sub join()
        t.Join()
    End Sub

    ' Execute one pending job in current thread.
    Public Function execute() As Boolean
        Dim c As Action = Nothing
        If q.pop(c) Then
            void_(c)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function wait(ByVal ms As Int64) As Boolean
        Return q.wait(ms)
    End Function

    Public Sub wait()
        q.wait()
    End Sub

    Public Function idle() As Boolean
        Return q.empty()
    End Function

    Public Function push(ByVal job As Action) As Boolean
        Return emplace(copy(job))
    End Function

    Public Function emplace(ByVal job As Action) As Boolean
        If job Is Nothing Then
            Return False
        Else
            q.emplace(job)
            Return True
        End If
    End Function

    Public Shared Operator +(ByVal this As slimqless2_runner, ByVal that As Action) As slimqless2_runner
        If this Is Nothing Then
            Return Nothing
        Else
            this.push(that)
            Return this
        End If
    End Operator

    Protected Overrides Sub Finalize()
        [stop]()
        MyBase.Finalize()
    End Sub
End Class
