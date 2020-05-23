﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Public Class lifetime_binder(Of T As Class)
    Public Shared ReadOnly instance As lifetime_binder(Of T) = Nothing
    Private ReadOnly s As object_unique_pointer_set(Of T) = Nothing

    Shared Sub New()
        instance = New lifetime_binder(Of T)()
    End Sub

    Protected Sub New()
        s = New object_unique_pointer_set(Of T)()
    End Sub

    Public Sub insert(ByVal i As T)
        assert(Not i Is Nothing)
        assert(s.insert(i))
    End Sub

    Public Sub [erase](ByVal i As T)
        assert(Not i Is Nothing)
        assert(s.erase(i))
    End Sub

    Public Sub clear()
        s.foreach(Sub(ByVal i As pointer(Of T))
                      disposable.dispose(+i)
                  End Sub)
        s.clear()
    End Sub

    Protected NotOverridable Overrides Sub Finalize()
        clear()
        MyBase.Finalize()
    End Sub
End Class

Public Class lifetime_binder
    Inherits lifetime_binder(Of Object)
End Class
