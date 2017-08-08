
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

    Private Declare Function GetWindowRect Lib "user32.dll" (ByVal wnd As HandleRef, ByRef rect As rect) As Boolean
    Private Declare Function GetWindowThreadProcessId Lib "user32.dll" _
        (ByVal wnd As HandleRef, ByRef process_id As UInt32) As UInt32

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

    Private Sub New()
    End Sub
End Class
