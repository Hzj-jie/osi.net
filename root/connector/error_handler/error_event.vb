
Imports osi.root.constants

Public Class error_event
    ' additional_jump is not accurate.
    Public Shared Event R1(ByVal err_type As error_type,
                           ByVal err_type_char As Char,
                           ByVal msg() As Object,
                           ByVal additional_jump As Int32)
    ' additional_jump is not accurate.
    Public Shared Event R2(ByVal err_type As error_type,
                           ByVal err_type_char As Char,
                           ByVal msg As String,
                           ByVal additional_jump As Int32)
    Public Shared Event R3(ByVal err_type As error_type, ByVal msg As String)
    Public Shared Event R4(ByVal msg As String)
    Public Shared Event R5(ByVal err_type As error_type)
    Public Shared Event R6(ByVal err_type As error_type, ByVal err_type_char As Char, ByVal msg As String)
    Public Shared Event A1()

    Private Shared ReadOnly r1lock As Object
    Private Shared ReadOnly r2lock As Object
    Private Shared ReadOnly r3lock As Object
    Private Shared ReadOnly r4lock As Object
    Private Shared ReadOnly r5lock As Object
    Private Shared ReadOnly r6lock As Object

    Shared Sub New()
        r1lock = New Object()
        r2lock = New Object()
        r3lock = New Object()
        r4lock = New Object()
        r5lock = New Object()
        r6lock = New Object()
    End Sub

    Public Shared Sub A()
        RaiseEvent A1()
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Shared Sub R(ByVal err_type As error_type,
                        ByVal err_type_char As Char,
                        ByVal msg() As Object,
                        ByVal additional_jump As Int32)
        Dim r1a As Boolean = False
        Dim r2a As Boolean = False
        Dim r3a As Boolean = False
        Dim r4a As Boolean = False
        Dim r5a As Boolean = False
        Dim r6a As Boolean = False
        r1a = event_attached(R1Event)
        r2a = event_attached(R2Event)
        r3a = event_attached(R3Event)
        r4a = event_attached(R4Event)
        r5a = event_attached(R5Event)
        r6a = event_attached(R6Event)
        Dim unattached As Boolean = False
        unattached = Not (r1a OrElse r2a OrElse r3a OrElse r4a OrElse r5a OrElse r6a)

        err_type_char = Char.ToLower(err_type_char)

        Dim merged_msg As String = Nothing
        If unattached OrElse r2a OrElse r3a OrElse r4a OrElse r6a Then
            merged_msg = error_message.P(msg)
        End If
        Dim full_msg As String = Nothing
        If unattached OrElse r3a OrElse r4a OrElse r6a Then
            If merged_msg Is Nothing Then
                assert_break()
            End If
            full_msg = error_message.P(err_type, err_type_char, merged_msg, additional_jump + 1)
        End If

        If unattached Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            ' Before global_init(), no event is attached, we at least should print the message to console.
            write_console_line(full_msg)
        End If

        SyncLock r1lock
            RaiseEvent R1(err_type, err_type_char, msg, additional_jump + 1)
        End SyncLock
        If r2a Then
            If merged_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r2lock
                RaiseEvent R2(err_type, err_type_char, merged_msg, additional_jump + 1)
            End SyncLock
        End If
        If r3a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r3lock
                RaiseEvent R3(err_type, full_msg)
            End SyncLock
        End If
        If r4a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r4lock
                RaiseEvent R4(full_msg)
            End SyncLock
        End If
        SyncLock r5lock
            RaiseEvent R5(err_type)
        End SyncLock
        If r6a Then
            If full_msg Is Nothing Then
                assert_break()
            End If
            SyncLock r6lock
                RaiseEvent R6(err_type, err_type_char, full_msg)
            End SyncLock
        End If
    End Sub
End Class
