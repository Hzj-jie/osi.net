
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Public Module _executor_ext
    <Extension> Public Function assert_execute_without_errors(ByVal this As executor) As Boolean
        If Not assertion.is_not_null(this) Then
            Return False
        End If
        this.execute()
        Return assertion.is_false(this.halt()) And assertions.of(this.errors()).empty()
    End Function
End Module
