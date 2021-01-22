
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

<global_init(global_init_level.debugging)>
Public Module _domain_unhandled_exception
    'do not set it to true unless you know what you are doing
    Public suspend_unhandled_exception As Boolean
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event domain_unhandled_exception(ByVal ex As Exception)
    Private ReadOnly handler As UnhandledExceptionEventHandler
    Private ReadOnly suspended_threads As vector(Of Thread)

    Sub New()
        handler = AddressOf handle
        suspended_threads = New vector(Of Thread)()
        application_lifetime.stopping_handle(Sub()
                                                 Dim i As UInt32 = 0
                                                 While i < suspended_threads.size()
                                                     suspended_threads(i).Interrupt()
                                                     i += uint32_1
                                                 End While
                                             End Sub)
        AddHandler AppDomain.CurrentDomain.UnhandledException, handler
    End Sub

    Private Sub init()
    End Sub

    'do not set suspend_unhandled_exception to true, unless you know what you are doing
    Public Sub enable_domain_unhandled_exception_handler(Optional ByVal suspend_unhandled_exception As Boolean = False)
        _domain_unhandled_exception.suspend_unhandled_exception = suspend_unhandled_exception
    End Sub

    Public Function suspended_threads_count() As Int64
        Return suspended_threads.size()
    End Function

    Public Function is_suspended_thread(ByVal t As Thread) As Boolean
        Dim i As UInt32 = 0
        While i < suspended_threads.size()
            If object_compare(suspended_threads(i), t) = 0 Then
                Return True
            End If
            i += uint32_1
        End While
        Return False
    End Function

    Private Sub raise_exception(ByVal arg As UnhandledExceptionEventArgs)
        Dim ex As Exception = Nothing
        ex = cast(Of Exception)(arg.ExceptionObject())
        log_unhandled_exception("unhandled domain exception occured: ", ex)
        RaiseEvent domain_unhandled_exception(ex)
    End Sub

    Private Sub handle(ByVal sender As Object, ByVal arg As UnhandledExceptionEventArgs)
        If suspend_unhandled_exception Then
            raise_exception(arg)
            suspended_threads.emplace_back(current_thread())
            current_thread().IsBackground() = True
            suspend()
        Else
            raise_exception(arg)
            assert_break()
        End If
        GC.KeepAlive(handler)   ' the handler was accessed in unmanaged code
    End Sub
End Module
