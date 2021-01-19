
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _assert
    Public Sub assert_break()
        If isdebugmode() AndAlso Not strongassert() Then
            attach_debugger()
        Else
            error_event.a()
            Environment.Exit(0)
        End If
    End Sub

    Private Function assert_failed(ByVal assertlevel As String, ByVal msg() As Object) As Boolean
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function assert_not_nothing_return(Of T)(ByVal i As T, ByVal ParamArray msgs() As Object) As T
        assert(Not i Is Nothing, msgs)
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function assert_return(Of T)(ByVal v As Boolean, ByVal i As T, ByVal ParamArray msgs() As Object) As T
        assert(v, msgs)
        Return i
    End Function
End Module
