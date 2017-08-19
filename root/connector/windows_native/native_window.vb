
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices
Imports osi.root.constants

Public NotInheritable Class native_window
    <StructLayout(LayoutKind.Sequential)>
    Public Structure rect
        Public left As Int32
        Public top As Int32
        Public right As Int32
        Public bottom As Int32

        Public Function width() As Int32
            Return right - left
        End Function

        Public Function height() As Int32
            Return bottom - top
        End Function
    End Structure

    Public Enum get_window_command As UInt32
        hwnd_first = 0
        hwnd_last = 1
        hwnd_next = 2
        hwnd_prev = 3
        owner = 4
        child = 5
        enable_popup = 6
    End Enum

    Public Enum get_ancestor_flag As UInt32
        parent = 1
        root = 2
        root_owner = 3
    End Enum

    Private Declare Function GetWindowRect Lib "user32.dll" (ByVal wnd As HandleRef, ByRef rect As rect) As Boolean
    Private Declare Function GetWindowThreadProcessId Lib "user32.dll" _
        (ByVal wnd As HandleRef, ByRef process_id As UInt32) As UInt32
    Private Declare Function GetWindow Lib "user32.dll" (ByVal wnd As HandleRef, ByVal cmd As UInt32) As IntPtr
    Private Declare Function GetAncestor Lib "user32.dll" (ByVal wnd As HandleRef, ByVal flag As UInt32) As IntPtr
    Private Declare Function GetClientRect Lib "user32.dll" (ByVal wnd As HandleRef, ByRef rect As rect) As Boolean

    Public Shared Function get_rect(ByVal wnd As HandleRef, ByRef rect As rect) As Boolean
        Try
            Return GetWindowRect(wnd, rect)
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute GetWindowRect() function, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function get_rect(ByVal wnd As Windows.Forms.Control, ByRef rect As rect) As Boolean
        If wnd Is Nothing Then
            Return False
        Else
            Return get_rect(New HandleRef(wnd, wnd.Handle()), rect)
        End If
    End Function

    Public Shared Function get_client_rect(ByVal wnd As HandleRef, ByRef rect As rect) As Boolean
        Try
            Return GetClientRect(wnd, rect)
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute GetClientRect() function, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function get_client_rect(ByVal wnd As Windows.Forms.Control, ByRef rect As rect) As Boolean
        If wnd Is Nothing Then
            Return False
        Else
            Return get_client_rect(New HandleRef(wnd, wnd.Handle()), rect)
        End If
    End Function

    Public Shared Function get_thread_process_id(ByVal wnd As HandleRef,
                                                 Optional ByRef process_id As UInt32 = 0,
                                                 Optional ByRef thread_id As UInt32 = 0) As Boolean
        Try
            thread_id = GetWindowThreadProcessId(wnd, process_id)
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute GetWindowThreadProcessId() function, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function get_thread_process_id(ByVal wnd As Windows.Forms.Control,
                                                 Optional ByRef process_id As UInt32 = 0,
                                                 Optional ByRef thread_id As UInt32 = 0) As Boolean
        Return get_thread_process_id(New HandleRef(wnd, wnd.Handle()), process_id, thread_id)
    End Function

    Public Shared Function get_window(ByVal wnd As HandleRef,
                                      ByVal cmd As get_window_command,
                                      ByRef result As IntPtr) As Boolean
        Try
            result = GetWindow(wnd, cmd)
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute GetWindow() function, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function get_window(ByVal wnd As Windows.Forms.Control,
                                      ByVal cmd As get_window_command,
                                      ByRef result As IntPtr) As Boolean
        Return get_window(New HandleRef(wnd, wnd.Handle()), cmd, result)
    End Function

    Public Shared Function get_ancestor(ByVal wnd As HandleRef,
                                        ByVal flag As get_ancestor_flag,
                                        ByRef result As IntPtr) As Boolean
        Try
            result = GetAncestor(wnd, flag)
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute GetAncestor() function, ex ", ex)
            Return False
        End Try
    End Function

    Public Shared Function get_ancestor(ByVal wnd As Windows.Forms.Control,
                                        ByVal flag As get_ancestor_flag,
                                        ByRef result As IntPtr) As Boolean
        Return get_ancestor(New HandleRef(wnd, wnd.Handle()), flag, result)
    End Function

    Private Sub New()
    End Sub
End Class
