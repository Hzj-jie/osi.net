﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class read_scoped(Of T)
    Private ReadOnly s As stack(Of T)

    Public Sub New()
        s = New stack(Of T)()
    End Sub

    Public Sub push(ByVal v As T)
        s.emplace(v)
    End Sub

    Public NotInheritable Class ref
        Implements IDisposable

        Private ReadOnly r As read_scoped(Of T)
        Public ReadOnly v As T

        Public Sub New(ByVal r As read_scoped(Of T))
            assert(Not r Is Nothing)
            assert(Not r.s.empty())
            Me.r = r
            Me.v = r.s.back()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            r.s.pop()
        End Sub
    End Class

    Public Function pop() As ref
        Return New ref(Me)
    End Function
End Class
