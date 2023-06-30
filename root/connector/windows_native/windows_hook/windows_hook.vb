
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices
Imports osi.root.constants

Public NotInheritable Class windows_hook
    Public Enum hook_id As Int32
        WH_CALLWNDPROC = 4
        WH_CALLWNDPROCRET = 12
        WH_CBT = 5
        WH_DEBUG = 9
        WH_FOREGROUNDIDLE = 11
        WH_GETMESSAGE = 3
        WH_JOURNALPLAYBACK = 1
        WH_JOURNALRECORD = 0
        WH_KEYBOARD = 2
        WH_KEYBOARD_LL = 13
        WH_MOUSE = 7
        WH_MOUSE_LL = 14
        WH_MSGFILTER = -1
        WH_SHELL = 10
        WH_SYSMSGFILTER = 6
    End Enum

    Public Delegate Function HOOKPROC(ByVal nCode As Int32, ByVal wParam As UIntPtr, ByVal lParam As IntPtr) As IntPtr

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall, SetLastError:=True)>
    Private Shared Function SetWindowsHookEx(ByVal idHook As Int32,
                                             ByVal lpfn As HOOKPROC,
                                             ByVal hMod As IntPtr,
                                             ByVal dwThreadId As UInt32) As IntPtr
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall, SetLastError:=True)>
    Private Shared Function UnhookWindowsHookEx(ByVal hhk As IntPtr) As Int32
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")>
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall, SetLastError:=True)>
    Private Shared Function CallNextHookEx(ByVal hhk As IntPtr,
                                           ByVal nCode As Int32,
                                           ByVal wParam As UIntPtr,
                                           ByVal lParam As IntPtr) As IntPtr
    End Function

    Public Shared Function [New](ByVal id As hook_id,
                                 ByVal f As HOOKPROC,
                                 ByVal [mod] As IntPtr,
                                 ByVal thread_id As UInt32) As auto_release_windows_hook
        Return New auto_release_windows_hook([set](id, f, [mod], thread_id), f)
    End Function

    Public Shared Function [set](ByVal id As hook_id,
                                 ByVal f As HOOKPROC,
                                 ByVal [mod] As IntPtr,
                                 ByVal thread_id As UInt32) As IntPtr
        Try
            Return SetWindowsHookEx(id, f, [mod], thread_id)
        Catch ex As Exception
            raise_error(error_type.warning, "SetWindowsHookEx cannot be found, ex ", ex)
            Return IntPtr.Zero
        End Try
    End Function

    Public Shared Function remove(ByVal hook As IntPtr) As Boolean
        Try
            Return UnhookWindowsHookEx(hook) <> 0
        Catch ex As Exception
            raise_error(error_type.warning, "UnhookWindowsHookEx cannot be found, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function forward_to(ByVal hook As IntPtr,
                                      ByVal code As Int32,
                                      ByVal w As UIntPtr,
                                      ByVal l As IntPtr) As IntPtr
        Try
            Return CallNextHookEx(hook, code, w, l)
        Catch ex As DllNotFoundException
            raise_error(error_type.warning, "CallNextHookEx cannot be found, ex ", ex)
            Return IntPtr.Zero
        End Try
    End Function

    Private Sub New()
    End Sub
End Class
