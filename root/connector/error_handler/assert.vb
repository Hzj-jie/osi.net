
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _assert
    ' The message should carry the stack trace, no reason to expose the exception type.
    Private NotInheritable Class assertion_break
        Inherits Exception

        Public Shared Sub at_here(ByVal msg As String)
            Throw New assertion_break(msg)
        End Sub

        Private Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
    End Class

    <ThreadStatic>
    Private ignore_assertion_failure As Boolean

    Public Sub ignore_assertion_break(ByVal ex As Exception)
        If TypeOf ex Is assertion_break Then
            Throw ex
        End If
    End Sub

    ' Slightly better name.
    Public Sub catch_assertion_failure(ByVal d As Action, ByVal msg As Action(Of String))
        expect_assertion_failure(d,
                                 Sub()
                                 End Sub,
                                 msg)
    End Sub

    Public Sub expect_assertion_failure(ByVal d As Action,
                                        ByVal not_reach As Action,
                                        ByVal check_exception As Action(Of String))
        If ignore_assertion_failure Then
            ' assert(Not ignore_assertion_failure)
            ' won't work here.
            assert_break()
        End If
        assert(Not d Is Nothing)
        assert(Not not_reach Is Nothing)
        assert(Not check_exception Is Nothing)
        ignore_assertion_failure = True
        Try
            d()
        Catch ex As assertion_break
            check_exception(ex.Message())
            Return
        Finally
            ignore_assertion_failure = False
        End Try
        not_reach()
    End Sub

    Public Sub assert_break()
        If isdebugmode() AndAlso Not strongassert() Then
            attach_debugger()
        Else
            error_event.a()
            Environment.Exit(exit_code.assertion_failure)
        End If
    End Sub

    Private Function assert_failed(ByVal assertlevel As String, ByVal msg() As Object) As Boolean
        Dim msgs() As Object = {assertlevel,
                                If(isemptyarray(msg), " failed.", " failed, "),
                                msg,
                                newline.incode(),
                                callstack()}
        ' This function should be rarely reached, the performance is less critical.
        If ignore_assertion_failure Then
            assertion_break.at_here(error_message.merge(msgs))
            ' Never reach
            Return False
        End If

        raise_error(error_type.critical, msgs)
        assert_break()
        Return False
    End Function

    Private Function assert_failed(ByVal msgs() As Object) As Boolean
        Return assert_failed("assert", msgs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function assert(ByVal v As Boolean) As Boolean
        Return v OrElse assert_failed(Nothing)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function assert(ByVal v As Boolean, ByVal ParamArray msgs() As Object) As Boolean
        Return v OrElse assert_failed(msgs)
    End Function
End Module
