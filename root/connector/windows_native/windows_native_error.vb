
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.InteropServices
Imports osi.root.constants

Public NotInheritable Class windows_native_error
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")>
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function GetLastError() As UInt32
    End Function

    Public Shared Function [get]() As UInt32
        Try
            Return GetLastError()
        Catch ex As Exception
            raise_error(error_type.warning, "Failed to execute GetLastError, ex ", ex)
            Return max_uint32
        End Try
    End Function

    Private Sub New()
    End Sub
End Class
