
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.constants

Public NotInheritable Class error_event
    ' additional_jump is not accurate.
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r1(ByVal err_type As error_type,
                           ByVal err_type_char As Char,
                           ByVal msg() As Object,
                           ByVal additional_jump As Int32)
    ' additional_jump is not accurate.
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r2(ByVal err_type As error_type,
                           ByVal err_type_char As Char,
                           ByVal msg As String,
                           ByVal additional_jump As Int32)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r3(ByVal err_type As error_type, ByVal msg As String)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r4(ByVal msg As String)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r5(ByVal err_type As error_type)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event r6(ByVal err_type As error_type, ByVal err_type_char As Char, ByVal msg As String)
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event a1()

    Private Shared ReadOnly r1lock As Object = New Object()
    Private Shared ReadOnly r2lock As Object = New Object()
    Private Shared ReadOnly r3lock As Object = New Object()
    Private Shared ReadOnly r4lock As Object = New Object()
    Private Shared ReadOnly r5lock As Object = New Object()
    Private Shared ReadOnly r6lock As Object = New Object()

    <global_init(global_init_level.log_and_counter_services + byte_1)>
    Private NotInheritable Class when_log_and_counter_services
        Public Shared executed As Boolean = False

        Private Shared Sub init()
            executed = True
        End Sub
    End Class

    <global_init(global_init_level.other)>
    Private NotInheritable Class replay_logs
        Private Shared ReadOnly replays As New List(Of Action)()

        Public Shared Sub add(ByVal replay As Action)
            SyncLock replays
                replays.Add(replay)
            End SyncLock
        End Sub

        Private Shared Sub init()
            SyncLock replays

            End SyncLock
        End Sub
    End Class

    Public Shared Sub a()
        RaiseEvent a1()
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Shared Sub r(ByVal err_type As error_type,
                        ByVal err_type_char As Char,
                        ByVal msg() As Object,
                        ByVal additional_jump As Int32)
        If Not when_log_and_counter_services.executed Then
            replay_logs.add(Sub()
                                r(err_type, err_type_char, msg, additional_jump)
                            End Sub)
            Return
        End If

        Dim r1a As Boolean = event_attached(r1Event)
        Dim r2a As Boolean = event_attached(r2Event)
        Dim r3a As Boolean = event_attached(r3Event)
        Dim r4a As Boolean = event_attached(r4Event)
        Dim r5a As Boolean = event_attached(r5Event)
        Dim r6a As Boolean = event_attached(r6Event)

        err_type_char = Char.ToLower(err_type_char)

        Dim merged_msg As String = Nothing
        If r2a OrElse r3a OrElse r4a OrElse r6a Then
            merged_msg = error_message.p(msg)
        End If
        Dim full_msg As String = Nothing
        If r3a OrElse r4a OrElse r6a Then
            If merged_msg Is Nothing Then
                assert_break()
            End If
            full_msg = error_message.p(err_type, err_type_char, merged_msg, additional_jump + 1)
        End If

        SyncLock r1lock
            RaiseEvent r1(err_type, err_type_char, msg, additional_jump + 1)
        End SyncLock
        If r2a Then
            If merged_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r2lock
                RaiseEvent r2(err_type, err_type_char, merged_msg, additional_jump + 1)
            End SyncLock
        End If
        If r3a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r3lock
                RaiseEvent r3(err_type, full_msg)
            End SyncLock
        End If
        If r4a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r4lock
                RaiseEvent r4(full_msg)
            End SyncLock
        End If
        SyncLock r5lock
            RaiseEvent r5(err_type)
        End SyncLock
        If r6a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r6lock
                RaiseEvent r6(err_type, err_type_char, full_msg)
            End SyncLock
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
