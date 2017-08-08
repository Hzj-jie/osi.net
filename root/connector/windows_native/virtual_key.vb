
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class virtual_key
    Private Declare Function MapVirtualKey _
        Lib "user32.dll" _
        Alias "MapVirtualKeyW" _
        (ByVal code As UInt32, ByVal map_type As UInt32) As UInt32

    Public Enum map_type As UInt32
        virtual_key_to_char = 2
        virtual_key_to_scan_code = 0
        scan_code_to_virtual_key = 1
        scan_code_to_virtual_key_ex = 3
    End Enum

    Public Shared Function map(ByVal code As UInt32, ByVal type As map_type) As UInt32
        Try
            Return MapVirtualKey(code, type)
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute MapVirtualKey(), ex ", ex)
            Return 0
        End Try
    End Function

    Private Sub New()
    End Sub
End Class
