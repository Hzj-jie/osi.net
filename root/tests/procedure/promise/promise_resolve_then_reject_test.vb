﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt

Public Class promise_resolve_then_reject_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim p As promise = Nothing
        p = New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                            resolve(New Object())
                            reject(New Object())
                        End Sub)
        p.then(Sub(ByVal result As Object)
                   assert(mre.force_set())
               End Sub,
               Sub(ByVal reason As Object)
                   assertion.is_true(False)
               End Sub)
        assert(mre.wait())
        Return True
    End Function
End Class
