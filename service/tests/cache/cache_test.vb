
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.cache
Imports osi.root.connector
Imports osi.root.utt

Public Class cache_test
    Inherits repeat_case_wrapper

    Private Shared ReadOnly round As Int64

    Shared Sub New()
        round = 1024 * 8 * If(isdebugbuild(), 1, 8)
    End Sub

    Public Sub New(ByVal s1 As icache(Of String, Byte()),
                   ByVal validate As Boolean)
        MyBase.New(New cache_case(s1, validate), round)
    End Sub
End Class
