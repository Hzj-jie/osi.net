
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics.CodeAnalysis
Imports System.Drawing
Imports System.Threading
Imports osi.root.constants

Public Module _console
    Public ReadOnly console_output_redirected As Boolean = Function() As Boolean
                                                               Try
                                                                   Dim x As Int32 = Console.CursorLeft()
                                                                   Return False
                                                               Catch ex As Exception
                                                                   Return True
                                                               End Try
                                                           End Function()

    <SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity")>
    Public Sub lock_console_output()
        Monitor.Enter(Console.Out)
    End Sub

    <SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity")>
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
        write_console(error_message.p(s))
    End Sub

    Private Sub rewrite(ByVal s As String, ByVal err As Boolean)
        If console_output_redirected Then
            If err Then
                write_console_error_line(s)
            Else
                write_console_line(s)
            End If
            Return
        End If
        If err Then
            lock_console_error_output()
        Else
            lock_console_output()
        End If

        Dim l As Int32 = Console.CursorLeft()
        Dim t As Int32 = Console.CursorTop()
        If err Then
            write_console_error(s)
        Else
            write_console(s)
        End If
        ' The last character in the line is ignored, otherwise the cursor will go to next line.
        If Console.BufferWidth() > 0 AndAlso
           strlen(s) Mod Console.BufferWidth() > 0 AndAlso
           strlen(s) Mod Console.BufferWidth() < (Console.BufferWidth() - 1) Then
            s = New String(character.blank, Console.BufferWidth() - (strlen_i(s) Mod Console.BufferWidth()) - 1)
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
        rewrite_console(error_message.p(s))
    End Sub

    Public Sub write_console_error(ByVal s As String)
        lock_console_error_output()
        Console.Error.Write(s)
        release_console_error_output()
    End Sub

    Public Sub write_console_error(ByVal ParamArray s() As Object)
        write_console_error(error_message.p(s))
    End Sub

    Public Sub rewrite_console_error(ByVal s As String)
        rewrite(s, True)
    End Sub

    Public Sub rewrite_console_error(ByVal ParamArray s() As Object)
        rewrite_console_error(error_message.p(s))
    End Sub

    Public Sub write_console_line(ByVal s As String)
        write_console(s + newline.incode())
    End Sub

    Public Sub write_console_line(ByVal ParamArray s() As Object)
        write_console_line(error_message.p(s))
    End Sub

    Public Sub write_console_error_line(ByVal s As String)
        write_console_error(s + newline.incode())
    End Sub

    Public Sub write_console_error_line(ByVal ParamArray s() As Object)
        write_console_error_line(error_message.p(s))
    End Sub

    Public Function closest_console_color(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As ConsoleColor
        Dim min_d As Int32 = max_int32
        Dim min_c As ConsoleColor = Nothing
        enum_def(Of ConsoleColor).foreach(Sub(ByVal cc As ConsoleColor, ByVal n As String)
                                              Dim c As Color = Nothing
                                              'no dark yellow in knowncolor enumeration
                                              c = Color.FromName(If(strsame(n, "DarkYellow", False), "Orange", n))
                                              Dim d As Int32 = (CShort(c.R) - r).power_2() +
                                                               (CShort(c.G) - g).power_2() +
                                                               (CShort(c.B) - b).power_2()
                                              If d = 0 OrElse d < min_d Then
                                                  min_c = cc
                                                  If d = 0 Then
                                                      break_lambda.at_here()
                                                  End If
                                              End If
                                          End Sub)
        Return min_c
    End Function
End Module
