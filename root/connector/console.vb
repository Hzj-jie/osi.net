
Imports System.Drawing
Imports System.Threading
Imports osi.root.constants

Public Module _console
    Public Sub lock_console_output()
        Monitor.Enter(Console.Out)
    End Sub

    Public Sub release_console_output()
        Monitor.Exit(Console.Out)
    End Sub

    Public Sub lock_console_error_output()
        lock_console_output()
    End Sub

    Public Sub release_console_error_output()
        release_console_output()
    End Sub

    Public Sub write_console(ByVal s As String)
        lock_console_output()
        Console.Write(s)
        release_console_output()
    End Sub

    Public Sub write_console(ByVal ParamArray s() As Object)
        write_console(error_message.P(s))
    End Sub

    Private Sub rewrite(ByVal s As String, ByVal err As Boolean)
        If err Then
            lock_console_error_output()
        Else
            lock_console_output()
        End If

        Dim l As Int32 = 0
        Dim t As Int32 = 0
        l = Console.CursorLeft()
        t = Console.CursorTop()
        If err Then
            write_console_error(s)
        Else
            write_console(s)
        End If
        ' The last character in the line is ignored, otherwise the cursor will go to next line.
        If Console.BufferWidth() > 0 AndAlso
           strlen(s) Mod Console.BufferWidth() > 0 AndAlso
           strlen(s) Mod Console.BufferWidth() < (Console.BufferWidth() - 1) Then
            s = New String(character.blank, Console.BufferWidth() - (strlen(s) Mod Console.BufferWidth()) - 1)
            If err Then
                write_console_error(s)
            Else
                write_console(s)
            End If
        End If
        Try
            Console.SetCursorPosition(l, t)
        Catch
        End Try

        If err Then
            release_console_error_output()
        Else
            release_console_output()
        End If
    End Sub

    Public Sub rewrite_console(ByVal s As String)
        rewrite(s, False)
    End Sub

    Public Sub rewrite_console(ByVal ParamArray s() As Object)
        rewrite_console(error_message.P(s))
    End Sub

    Public Sub write_console_error(ByVal s As String)
        lock_console_error_output()
        Console.Error.Write(s)
        release_console_error_output()
    End Sub

    Public Sub write_console_error(ByVal ParamArray s() As Object)
        write_console_error(error_message.P(s))
    End Sub

    Public Sub rewrite_console_error(ByVal s As String)
        rewrite(s, True)
    End Sub

    Public Sub rewrite_console_error(ByVal ParamArray s() As Object)
        rewrite_console_error(error_message.P(s))
    End Sub

    Public Sub write_console_line(ByVal s As String)
        write_console(s + newline.incode())
    End Sub

    Public Sub write_console_line(ByVal ParamArray s() As Object)
        write_console_line(error_message.P(s))
    End Sub

    Public Sub write_console_error_line(ByVal s As String)
        write_console_error(s + newline.incode())
    End Sub

    Public Sub write_console_error_line(ByVal ParamArray s() As Object)
        write_console_error_line(error_message.P(s))
    End Sub

    Public Function closest_console_color(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As ConsoleColor
        Dim min_d As Int32 = 0
        min_d = max_int32
        Dim min_c As ConsoleColor = Nothing
        assert(enum_traversal(Of ConsoleColor)(Function(cc As ConsoleColor, n As String) As Boolean
                                                   Dim c As Color = Nothing
                                                   'no dark yellow in knowncolor enumeration
                                                   c = Color.FromName(If(strsame(n, "DarkYellow", False),
                                                                         "Orange",
                                                                         n))
                                                   Dim d As Int32 = 0
                                                   d = (CShort(c.R) - r).power_2() +
                                                       (CShort(c.G) - g).power_2() +
                                                       (CShort(c.B) - b).power_2()
                                                   If d = 0 OrElse
                                                      d < min_d Then
                                                       min_c = cc
                                                       Return d <> 0
                                                   Else
                                                       Return True
                                                   End If
                                               End Function))
        Return min_c
    End Function
End Module
