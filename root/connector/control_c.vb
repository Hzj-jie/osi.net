
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.constants

<global_init(global_init_level.foundamental)>
Public NotInheritable Class control_c
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Shared Event press(ByRef cancel As Boolean)
    Private Shared _pressed As Boolean = False
    Private Shared _enabled As Boolean = True
    Private Shared _blocking As Int32 = 0
    Private Shared _exit_process As Boolean = True
    Private Shared _exit_code As Int32 = constants.exit_code.succeeded

    Public Shared Function enabled() As Boolean
        Return _enabled
    End Function

    Public Shared Sub enable()
        _enabled = True
    End Sub

    Public Shared Sub disable()
        _enabled = False
    End Sub

    Public Shared Function process_will_exit() As Boolean
        Return _exit_process
    End Function

    Public Shared Sub exit_process()
        _exit_process = True
    End Sub

    Public Shared Sub do_not_exit_process()
        _exit_process = False
    End Sub

    Public Shared Sub block()
        assert(Interlocked.Increment(_blocking) > 0)
    End Sub

    Public Shared Sub release()
        assert(Interlocked.Decrement(_blocking) >= 0)
    End Sub

    Public Shared Function blocking_count() As Int32
        Return _blocking
    End Function

    Public Shared Function blocking() As Boolean
        Return blocking_count() > 0
    End Function

    Public Shared Function pressed() As Boolean
        Return _pressed
    End Function

    Public Shared Sub exit_code(ByVal c As Int32)
        _exit_code = c
    End Sub

    Public Shared Function exit_code() As Int32
        Return _exit_code
    End Function

    Private Shared Sub init()
        AddHandler Console.CancelKeyPress,
                   Sub(ByVal sender As Object, ByVal arg As ConsoleCancelEventArgs)
                       If Not enabled() Then
                           Return
                       End If
                       _pressed = True
                       RaiseEvent press(arg.Cancel())
                       If Not arg.Cancel() Then
                           While blocking()
                               sleep()
                           End While
                           If process_will_exit() Then
                               ' Trigger normal exit process
                               Environment.Exit(exit_code())
                           Else
                               arg.Cancel() = True
                           End If
                       End If
                       _pressed = False
                   End Sub
    End Sub

    Private Sub New()
    End Sub
End Class
