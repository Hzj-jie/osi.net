
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _assert
    Private NotInheritable Class assertion_break
        Inherits Exception

        Private Shared ReadOnly instance As New assertion_break()

        Public Shared Sub at_here()
            Throw instance
        End Sub

        Private Sub New()
        End Sub
    End Class

    <ThreadStatic>
    Private ignore_assertion_failure As Boolean

    Public Sub expect_assertion_failure(ByVal d As Action, ByVal check As Action(Of Boolean))
        If ignore_assertion_failure Then
            ' assert(Not ignore_assertion_failure)
            ' won't work here.
            assert_break()
        End If
        assert(Not d Is Nothing)
        assert(Not check Is Nothing)
        ignore_assertion_failure = True
        Try
            d()
        Catch ex As assertion_break
            Return
        Finally
            ignore_assertion_failure = False
        End Try
        check(False)
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
        ' This function should be rearly reached, the performance is less critical.
        If ignore_assertion_failure Then
            assertion_break.at_here()
            ' Never reach
            Return False
        End If

        raise_error(error_type.critical,
                    assertlevel,
                    If(isemptyarray(msg), " failed.", " failed, "),
                    msg,
                    newline.incode(),
                    callstack())
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
