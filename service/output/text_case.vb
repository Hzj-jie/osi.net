
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utils

Friend Module _text_case
    Private Function valid_text_action(ByVal a As action) As Boolean
        Return a = action.text_output OrElse
               a = action.text_error_output OrElse
               a = action.cursor_position OrElse
               a = action.change_foreground_color OrElse
               a = action.change_background_color
    End Function

    Private Function valid_text_output_meta(ByVal i() As Byte) As Boolean
        'for performance concern
        Return Not isemptyarray(i)
    End Function

    Private Function valid_text_error_output_meta(ByVal i() As Byte) As Boolean
        Return valid_text_output_meta(i)
    End Function

    Private Function valid_text_cursor_position_meta(ByVal i() As Byte) As Boolean
        Return array_size(i) = sizeof_int32 * 2
    End Function

    Private Function valid_text_change_color_meta(ByVal i() As Byte) As Boolean
        Return array_size(i) = 4
    End Function

    Private Function valid_text_change_foreground_color_meta(ByVal i() As Byte) As Boolean
        Return valid_text_change_color_meta(i)
    End Function

    Private Function valid_text_change_background_color_meta(ByVal i() As Byte) As Boolean
        Return valid_text_change_color_meta(i)
    End Function

    Private Function valid_text_meta(ByVal a As action, ByVal i() As Byte) As Boolean
        If a = action.text_output Then
            Return valid_text_output_meta(i)
        ElseIf a = action.text_error_output Then
            Return valid_text_error_output_meta(i)
        ElseIf a = action.cursor_position Then
            Return valid_text_cursor_position_meta(i)
        ElseIf a = action.change_foreground_color Then
            Return valid_text_change_foreground_color_meta(i)
        ElseIf a = action.change_background_color Then
            Return valid_text_change_background_color_meta(i)
        Else
            Return assert(False)
        End If
    End Function

    <Extension()> Public Function valid_text_case(ByVal c As [case]) As Boolean
        Return Not c Is Nothing AndAlso
               c.mode = mode.text AndAlso
               valid_text_action(c.action) AndAlso
               valid_text_meta(c.action, c.meta)
    End Function

    <Extension()> Public Function text(ByVal c As [case], ByRef o As String) As Boolean
        If c Is Nothing Then
            Return False
        Else
            o = bytes_str(c.meta)
            Return True
        End If
    End Function

    Private Function text_meta(ByVal s As String) As Byte()
        Return str_bytes(s)
    End Function

    <Extension()> Public Function cursor_position(ByVal c As [case],
                                                  ByRef x As UInt32,
                                                  ByRef y As UInt32) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Dim p As UInt32 = 0
            Return bytes_uint32(c.meta, x, p) AndAlso
                   bytes_uint32(c.meta, y, p)
        End If
    End Function

    Private Function cursor_position_meta(ByVal x As UInt32, ByVal y As UInt32) As Byte()
        Dim p As UInt32 = 0
        Dim r() As Byte = Nothing
        ReDim r((sizeof_int32 << 1) - 1)
        assert(uint32_bytes(x, r, p) AndAlso uint32_bytes(y, r, p))
        Return r
    End Function

    Private Function color(ByVal c As [case],
                           ByRef r As Byte,
                           ByRef g As Byte,
                           ByRef b As Byte,
                           ByRef a As Byte) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Dim p As UInt32 = 0
            Return bytes_byte(c.meta, r, p) AndAlso
                   bytes_byte(c.meta, g, p) AndAlso
                   bytes_byte(c.meta, b, p) AndAlso
                   bytes_byte(c.meta, a, p)
        End If
    End Function

    Private Function color_meta(ByVal r As Byte,
                                ByVal g As Byte,
                                ByVal b As Byte,
                                ByVal a As Byte) As Byte()
        Dim p As UInt32 = 0
        Dim rs() As Byte = Nothing
        ReDim rs((sizeof_int8 << 2) - 1)
        assert(byte_bytes(r, rs, p) AndAlso
               byte_bytes(g, rs, p) AndAlso
               byte_bytes(b, rs, p) AndAlso
               byte_bytes(a, rs, p))
        Return rs
    End Function

    <Extension()> Public Function foreground_color(ByVal c As [case],
                                                   ByRef r As Byte,
                                                   ByRef g As Byte,
                                                   ByRef b As Byte,
                                                   ByRef a As Byte) As Boolean
        Return color(c, r, g, b, a)
    End Function

    <Extension()> Public Function background_color(ByVal c As [case],
                                                   ByRef r As Byte,
                                                   ByRef g As Byte,
                                                   ByRef b As Byte,
                                                   ByRef a As Byte) As Boolean
        Return color(c, r, g, b, a)
    End Function

    Public Function text_case(ByVal action As action, ByVal meta() As Byte) As [case]
        Dim r As [case] = Nothing
        r = New [case](mode.text, action, meta)
        assert(valid_text_case(r))
        Return r
    End Function

    Public Function text_output_case(ByVal s As String) As [case]
        Return text_case(action.text_output, text_meta(s))
    End Function

    Public Function text_error_output_case(ByVal s As String) As [case]
        Return text_case(action.text_error_output, text_meta(s))
    End Function

    Public Function text_cursor_position_case(ByVal x As Int32, ByVal y As Int32) As [case]
        Return text_case(action.cursor_position, cursor_position_meta(x, y))
    End Function

    Public Function text_change_foreground_color_case(ByVal r As Byte,
                                                      ByVal g As Byte,
                                                      ByVal b As Byte,
                                                      ByVal a As Byte) As [case]
        Return text_case(action.change_foreground_color, color_meta(r, g, b, a))
    End Function

    Public Function text_change_background_color_case(ByVal r As Byte,
                                                      ByVal g As Byte,
                                                      ByVal b As Byte,
                                                      ByVal a As Byte) As [case]
        Return text_case(action.change_background_color, color_meta(r, g, b, a))
    End Function
End Module
