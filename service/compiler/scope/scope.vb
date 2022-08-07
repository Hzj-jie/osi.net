﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.constructor

Partial Public Class scope(Of T As scope(Of T))
    Implements IDisposable

    <ThreadStatic> Private Shared in_thread As T
    Protected ReadOnly parent As T
    ' This variable has no functionality, but only ensures the start_scope() won't be executed multiple times.
    Private child As T = Nothing
    Private ReadOnly this As lazier(Of T) = lazier.of(Function() As T
                                                          Return direct_cast(Of T)(Me)
                                                      End Function)
    Private ReadOnly root As lazier(Of T) = lazier.of(Function() As T
                                                          Dim s As T = +this
                                                          While Not s.is_root()
                                                              s = s.parent
                                                          End While
                                                          Return s
                                                      End Function)
    Private ReadOnly ds As New vector(Of Action)()

    Protected Sub New(ByVal parent As T)
        Me.parent = parent
        If parent Is Nothing Then
            assert(in_thread Is Nothing)
        Else
            assert(object_compare(in_thread, parent) = 0)
        End If
        in_thread = +this
    End Sub

    Public Function start_scope() As T
        assert(child Is Nothing)
        child = inject_constructor(Of T).invoke(Me)
        Return child
    End Function

    Public Sub when_end_scope(ByVal a As Action)
        assert(Not a Is Nothing)
        ds.emplace_back(a)
    End Sub

    Public Function without_end_scope() As IDisposable
        Return defer.to(Sub()
                            in_thread = Nothing
                        End Sub)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        assert(object_compare(in_thread, Me) = 0)
        Dim i As Int64 = ds.size() - 1
        While i >= 0
            ds(CUInt(i))()
            i -= 1
        End While
        If Not parent Is Nothing Then
            assert(object_compare(parent.child, Me) = 0)
            parent.child = Nothing
        End If
        in_thread = parent
    End Sub

    Public Shared Function current() As T
        assert(Not in_thread Is Nothing)
        Return in_thread
    End Function

    ' Avoid naming conflict.
    Public Shared Function current_scope() As scope(Of T)
        Return current()
    End Function

    Protected Function is_root() As Boolean
        Return parent Is Nothing
    End Function

    Protected Function from_root(Of RT)(ByVal getter As Func(Of T, RT)) As RT
        assert(Not getter Is Nothing)
        If is_root() Then
            assert(Not getter(+this) Is Nothing)
            Return getter(+this)
        End If
        assert(getter(+this) Is Nothing)
        Return (+root).from_root(getter)
    End Function
End Class
