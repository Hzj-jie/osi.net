
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.iosys

Public Class console_receiver
    Implements ireceiver(Of [case])

    Public Function receive(ByVal c As [case]) As event_comb Implements ireceiver(Of [case]).receive
        Return sync_async(Function() As Boolean
                              If c.valid_text_case() Then
                                  Dim s As String = Nothing
                                  Dim x As UInt32 = 0
                                  Dim y As UInt32 = 0
                                  Dim r As Byte = 0
                                  Dim g As Byte = 0
                                  Dim b As Byte = 0
                                  Dim a As Byte = 0
                                  If c.text(s) Then
                                      Console.Write(s)
                                      Return True
                                  ElseIf c.cursor_position(x, y) Then
                                      If x < Console.BufferWidth() AndAlso
                                         y < Console.BufferHeight() Then
                                          Console.SetCursorPosition(x, y)
                                          Return True
                                      Else
                                          Return False
                                      End If
                                  ElseIf c.foreground_color(r, g, b, a) Then
                                      Console.ForegroundColor() = closest_console_color(r, g, b)
                                      Return True
                                  ElseIf c.background_color(r, g, b, a) Then
                                      Console.BackgroundColor() = closest_console_color(r, g, b)
                                      Return True
                                  Else
                                      Return False
                                  End If
                              Else
                                  Return False
                              End If
                          End Function)
    End Function
End Class
