
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices
Imports osi.root.constants

Public NotInheritable Class lowlevel_keyboard_hook
    Public Const HC_ACTION As Int32 = 0

    Public Enum flag As Int32
        KF_EXTENDED = &H100
        KF_DLGMODE = &H800
        KF_MENUMODE = &H1000
        KF_ALTDOWN = &H2000
        KF_REPEAT = &H4000
        KF_UP = &H8000

        LLKHF_EXTENDED = (KF_EXTENDED >> 8)
        LLKHF_LOWER_IL_INJECTED = &H2
        LLKHF_INJECTED = &H10
        LLKHF_ALTDOWN = (KF_ALTDOWN >> 8)
        LLKHF_UP = (KF_UP >> 8)
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As UInt32
        Public scanCode As UInt32
        Public flags As UInt32
        Public time As UInt32
        <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")>
        Public dwExtraInfo As UIntPtr
    End Structure

    Public Enum event_type
        unknown
        key_down
        key_up
        sys_key_down
        sys_key_up
    End Enum

    Public Class [event]
        Public ReadOnly type As event_type
        Public ReadOnly virtual_keycode As UInt32
        Public ReadOnly scan_code As UInt32
        Public ReadOnly raw_flags As UInt32
        Public ReadOnly raw_time As UInt32
        Public ReadOnly extra_info As UIntPtr

        Public ReadOnly time As Date
        Public ReadOnly extended As Boolean
        Public ReadOnly lower_integrity_level_injected As Boolean
        Public ReadOnly injected As Boolean
        Public ReadOnly alt_down As Boolean
        Public ReadOnly key_up As Boolean

        Private Shared Function parse_wParam(ByVal wParam As UIntPtr) As event_type
            Select Case wParam.ToUInt32()
                Case windows_message.WM_KEYDOWN
                    Return event_type.key_down
                Case windows_message.WM_KEYUP
                    Return event_type.key_up
                Case windows_message.WM_SYSKEYDOWN
                    Return event_type.sys_key_down
                Case windows_message.WM_SYSKEYUP
                    Return event_type.sys_key_up
                Case Else
                    Return event_type.unknown
            End Select
        End Function

        Private Function test(ByVal f As flag) As Boolean
            Return (Me.raw_flags And f) = f
        End Function

        Private Sub New(ByVal type As event_type, ByVal s As KBDLLHOOKSTRUCT)
            Me.type = type
            Me.virtual_keycode = s.vkCode
            Me.scan_code = s.scanCode
            Me.raw_flags = s.flags
            Me.raw_time = s.time
            Me.extra_info = s.dwExtraInfo

            Me.time = low_res_ticks_retriever.to_date(Me.raw_time)
            Me.extended = test(flag.LLKHF_EXTENDED)
            Me.lower_integrity_level_injected = test(flag.LLKHF_LOWER_IL_INJECTED)
            Me.injected = test(flag.LLKHF_INJECTED)
            Me.alt_down = test(flag.LLKHF_ALTDOWN)
            Me.key_up = test(flag.LLKHF_UP)

            ' TODO: What's in the dwExtraInfo?
        End Sub

        Public Sub New(ByVal wParam As UIntPtr, ByVal lParam As IntPtr)
            Me.New(parse_wParam(wParam),
                   DirectCast(Marshal.PtrToStructure(lParam, GetType(KBDLLHOOKSTRUCT)), KBDLLHOOKSTRUCT))
        End Sub
    End Class

    Private Shared Function hook_proc(ByVal f As Func(Of [event], Boolean)) As windows_hook.HOOKPROC
        If f Is Nothing Then
            Return Nothing
        End If
        Return Function(ByVal nCode As Int32,
                        ByVal wParam As UIntPtr,
                        ByVal lParam As IntPtr) As IntPtr
                   If nCode = HC_ACTION AndAlso f(New [event](wParam, lParam)) Then
                       Return IntPtr.Add(IntPtr.Zero, 1)
                   Else
                       Return windows_hook.forward_to(IntPtr.Zero, nCode, wParam, lParam)
                   End If
               End Function
    End Function

    Public Shared Function [New](ByVal f As Func(Of [event], Boolean),
                                 ByVal [mod] As IntPtr,
                                 ByVal thread_id As UInt32) As auto_release_windows_hook
        Return windows_hook.[New](windows_hook.hook_id.WH_KEYBOARD_LL, hook_proc(f), [mod], thread_id)
    End Function

    Public Shared Function [New](ByVal f As Func(Of [event], Boolean),
                                 ByVal thread_id As UInt32) As auto_release_windows_hook
        Return windows_hook.[New](windows_hook.hook_id.WH_KEYBOARD_LL, hook_proc(f), IntPtr.Zero, thread_id)
    End Function

    Public Shared Function [New](ByVal f As Func(Of [event], Boolean)) As auto_release_windows_hook
        Return windows_hook.[New](windows_hook.hook_id.WH_KEYBOARD_LL, hook_proc(f), IntPtr.Zero, uint32_0)
    End Function

    Public Shared Function remove(ByVal hook As IntPtr) As Boolean
        Return windows_hook.remove(hook)
    End Function

    Private Sub New()
    End Sub
End Class
