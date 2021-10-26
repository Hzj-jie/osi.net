
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Public Module _executor_ext
    <Extension> Public Function assert_execute_without_errors(ByVal this As executor) As Boolean
        assert(Not this Is Nothing)
        this.execute()
        Return assertion.is_false(this.halt()) AndAlso assertion.vector_empty(this.errors())
    End Function
End Module
