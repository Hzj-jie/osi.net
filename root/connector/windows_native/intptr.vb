
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _intptr
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hHandle As IntPtr) As Boolean

    <Extension()> Public Function close_as_handle(ByVal h As IntPtr) As Boolean
        Try
            Return CloseHandle(h)
        Catch ex As DllNotFoundException
            Return False
        End Try
    End Function
End Module
