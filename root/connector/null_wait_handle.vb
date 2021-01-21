﻿
Imports System.Threading

' An already passed WaitHandle.
Public NotInheritable Class null_wait_handle
    Inherits WaitHandle

    Public Shared ReadOnly instance As null_wait_handle

    Shared Sub New()
        instance = New null_wait_handle()
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function WaitOne() As Boolean
        Return True
    End Function

    Public Overrides Function WaitOne(ByVal millisecondsTimeout As Int32) As Boolean
        Return True
    End Function

    Public Overrides Function WaitOne(ByVal millisecondsTimeout As Int32, ByVal exitContext As Boolean) As Boolean
        Return True
    End Function

    Public Overrides Function WaitOne(ByVal timeout As TimeSpan) As Boolean
        Return True
    End Function

    Public Overrides Function WaitOne(ByVal timeout As TimeSpan, ByVal exitContext As Boolean) As Boolean
        Return True
    End Function
End Class
