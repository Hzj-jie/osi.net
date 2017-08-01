
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class windows_message
    Public Const WM_KEYDOWN As UInt32 = &H100
    Public Const WM_KEYUP As UInt32 = &H101
    Public Const WM_SYSKEYDOWN As UInt32 = &H104
    Public Const WM_SYSKEYUP As UInt32 = &H105

    Private Sub New()
    End Sub
End Class
