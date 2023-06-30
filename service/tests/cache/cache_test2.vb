
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.cache
Imports osi.root.connector
Imports osi.root.utt

Public Class cache_test2
    Inherits repeat_case_wrapper

    Private Shared ReadOnly round As Int64

    Shared Sub New()
        round = 1024 * 8 * If(isdebugbuild(), 1, 8)
    End Sub

    Public Sub New(ByVal s1 As islimcache2(Of Int32, Int32))
        MyBase.New(New cache_case2(s1), round)
    End Sub
End Class
