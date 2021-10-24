﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class read_scoped(Of T)
        Private ReadOnly s As stack(Of T)
        Private pending_dispose As UInt32

        Public Sub New()
            s = New stack(Of T)()
            pending_dispose = 0
        End Sub

        Public Sub push(ByVal v As T)
            s.emplace(v)
        End Sub

        Public Class ref(Of RT)
            Implements IDisposable

            Private ReadOnly r As read_scoped(Of T)
            Private ReadOnly f As _do_val_ref(Of T, RT, Boolean)

            Public Sub New(ByVal r As read_scoped(Of T), ByVal f As _do_val_ref(Of T, RT, Boolean))
                assert(Not r Is Nothing)
                assert(Not r.s.empty())
                assert(Not f Is Nothing)
                Me.r = r
                Me.f = f
                r.pending_dispose += uint32_1
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                r.pending_dispose -= uint32_1
                r.s.pop()
            End Sub

            Public Function retrieve(ByRef o As RT) As Boolean
                Return f(r.s.back(), o)
            End Function

            Public Shared Operator +(ByVal this As ref(Of RT)) As RT
                assert(Not this Is Nothing)
                Dim r As RT = Nothing
                assert(this.retrieve(r))
                Return r
            End Operator
        End Class

        Public NotInheritable Class ref
            Inherits ref(Of T)

            Public Sub New(ByVal r As read_scoped(Of T))
                MyBase.New(r,
                           Function(ByVal x As T, ByRef o As T) As Boolean
                               o = x
                               Return True
                           End Function)
            End Sub
        End Class

        Public Function pop() As ref
            assert(size() > pending_dispose)
            Return New ref(Me)
        End Function

        Public Function pop(Of RT)(ByVal f As _do_val_ref(Of T, RT, Boolean)) As ref(Of RT)
            assert(size() > pending_dispose)
            Return New ref(Of RT)(Me, f)
        End Function

        Public Function size() As UInt32
            Return s.size()
        End Function
    End Class
End Namespace
