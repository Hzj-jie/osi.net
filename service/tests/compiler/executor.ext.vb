
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Public Module _executor_ext
    <MethodImpl(MethodImplOptions.NoInlining)>
    <Extension()> Public Function assert_execute_without_errors(ByVal this As executor) As Boolean
        Return assert_execute_without_errors(this, backtrace())
    End Function

    <Extension()> Public Function assert_execute_without_errors(ByVal this As executor, ByVal msg As Object) As Boolean
        If Not assertion.is_not_null(this, msg) Then
            Return False
        End If
        this.execute()
        Return assertion.is_false(this.halt(), msg) And assertions.of(this.errors()).empty(msg)
    End Function
End Module
