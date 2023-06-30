
Imports osi.service.iosys

Public Class text_agent
    Inherits agent(Of [case])

    Public Sub output(ByVal s As String)
        received(text_output_case(s))
    End Sub

    Public Sub error_output(ByVal s As String)
        received(text_error_output_case(s))
    End Sub

    Public Sub cursor_position(ByVal x As Int32, ByVal y As Int32)
        received(text_cursor_position_case(x, y))
    End Sub

    Public Sub change_foreground_color(ByVal r As Byte,
                                       ByVal g As Byte,
                                       ByVal b As Byte,
                                       ByVal a As Byte)
        received(text_change_foreground_color_case(r, g, b, a))
    End Sub

    Public Sub change_background_color(ByVal r As Byte,
                                       ByVal g As Byte,
                                       ByVal b As Byte,
                                       ByVal a As Byte)
        received(text_change_background_color_case(r, g, b, a))
    End Sub
End Class
