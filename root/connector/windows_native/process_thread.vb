
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _process_thread
    <Flags()> _
    Public Enum ThreadAccess As Int32
        TERMINATE = (&H1)
        SUSPEND_RESUME = (&H2)
        GET_CONTEXT = (&H8)
        SET_CONTEXT = (&H10)
        SET_INFORMATION = (&H20)
        QUERY_INFORMATION = (&H40)
        SET_THREAD_TOKEN = (&H80)
        IMPERSONATE = (&H100)
        DIRECT_IMPERSONATION = (&H200)
    End Enum

    Private Declare Function OpenThread _
                             Lib "kernel32.dll" _
                             (ByVal dwDesiredAccess As ThreadAccess,
                              ByVal bInheritHandle As Boolean,
                              ByVal dwThreadId As UInt32) As IntPtr
    Private Declare Function SuspendThread Lib "kernel32.dll" (ByVal hThread As IntPtr) As UInt32
    Private Declare Function ResumeThread Lib "kernel32.dll" (ByVal hThread As IntPtr) As UInt32

    Public Function open_thread(ByVal access As ThreadAccess,
                                ByVal inherit_handle As Boolean,
                                ByVal thread_id As UInt32,
                                ByRef o As IntPtr) As Boolean
        Try
            o = OpenThread(access, inherit_handle, thread_id)
            Return o <> IntPtr.Zero
        Catch ex As DllNotFoundException
            Return False
        End Try
    End Function

    Public Function open_thread(ByVal access As ThreadAccess,
                                ByVal inherit_handle As Boolean,
                                ByVal thread_id As UInt32) As IntPtr
        Dim r As IntPtr = Nothing
        If open_thread(access, inherit_handle, thread_id, r) Then
            Return r
        End If
        Return IntPtr.Zero
    End Function

    Public Function open_thread(ByVal access As ThreadAccess,
                                ByVal thread_id As UInt32,
                                ByRef o As IntPtr) As Boolean
        Return open_thread(access, False, thread_id, o)
    End Function

    Public Function open_thread(ByVal access As ThreadAccess, ByVal thread_id As UInt32) As IntPtr
        Dim r As IntPtr = Nothing
        If open_thread(access, thread_id, r) Then
            Return r
        End If
        Return IntPtr.Zero
    End Function

    Public Function suspend_thread(ByVal h As IntPtr) As UInt32
        Try
            Return SuspendThread(h)
        Catch ex As DllNotFoundException
            Return max_uint32
        End Try
    End Function

    Public Function resume_thread(ByVal h As IntPtr) As UInt32
        Try
            Return ResumeThread(h)
        Catch ex As DllNotFoundException
            Return max_uint32
        End Try
    End Function

    <Extension()> Public Function dw_thread_id(ByVal t As ProcessThread) As UInt32
        Return int32_uint32(t.Id())
    End Function

    <Extension()> Public Function open_thread(ByVal t As ProcessThread,
                                              ByVal access As ThreadAccess,
                                              ByVal inherit_handle As Boolean,
                                              ByRef o As IntPtr) As Boolean
        If t Is Nothing Then
            Return False
        End If
        Return open_thread(access, inherit_handle, t.dw_thread_id(), o)
    End Function

    <Extension()> Public Function open_thread(ByVal t As ProcessThread,
                                              ByVal access As ThreadAccess,
                                              ByVal inherit_handle As Boolean) As IntPtr
        If t Is Nothing Then
            Return IntPtr.Zero
        End If
        Return open_thread(access, inherit_handle, t.dw_thread_id())
    End Function

    <Extension()> Public Function open_thread(ByVal t As ProcessThread,
                                              ByVal access As ThreadAccess,
                                              ByRef o As IntPtr) As Boolean
        If t Is Nothing Then
            Return False
        End If
        Return open_thread(access, t.dw_thread_id(), o)
    End Function

    <Extension()> Public Function open_thread(ByVal t As ProcessThread, ByVal access As ThreadAccess) As IntPtr
        If t Is Nothing Then
            Return IntPtr.Zero
        End If
        Return open_thread(access, t.dw_thread_id())
    End Function

    <Extension()> Public Function suspend_thread(ByVal t As ProcessThread) As Boolean
        Dim handle As IntPtr = Nothing
        If t.open_thread(ThreadAccess.SUSPEND_RESUME, handle) Then
            Dim r As Boolean = False
            r = (SuspendThread(handle) <> max_uint32)
            Return handle.close_as_handle() AndAlso r
        End If
        Return False
    End Function

    <Extension()> Public Function resume_thread(ByVal t As ProcessThread) As Boolean
        Dim handle As IntPtr = Nothing
        If t.open_thread(ThreadAccess.SUSPEND_RESUME, handle) Then
            Dim r As Boolean = False
            r = (resume_thread(handle) <> max_uint32)
            Return handle.close_as_handle() AndAlso r
        End If
        Return False
    End Function

    <Extension()> Public Function dispose(ByVal p As ProcessThreadCollection) As Boolean
        If p Is Nothing Then
            Return False
        End If
        Dim r As Boolean = True
        For i As Int32 = 0 To p.Count() - 1
            r = r And p(i).not_null_and_dispose()
        Next
        Return r
    End Function
End Module
