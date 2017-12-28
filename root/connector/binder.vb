﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _binder
    <Extension()> Public Function has_value(Of T As Class, PROTECTOR) _
                                           (ByVal i As binder(Of T, PROTECTOR)) As Boolean
        Return (Not i Is Nothing AndAlso i.has_local_value()) OrElse
               binder(Of T, PROTECTOR).has_global_value()
    End Function
End Module

Public Class binder
    Public Shared Function [New](Of T As Class, PROTECTOR)(ByVal i As T) As binder(Of T, PROTECTOR)
        Dim r As binder(Of T, PROTECTOR) = Nothing
        r = New binder(Of T, PROTECTOR)(i)
        Return r
    End Function

    Public Shared Function [New](Of T As Class)(ByVal i As T) As binder(Of T)
        Dim r As binder(Of T) = Nothing
        r = New binder(Of T)(i)
        Return r
    End Function
End Class

Public Class binder(Of T As Class)
    Inherits binder(Of T, Object)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal i As T)
        MyBase.New(i)
    End Sub
End Class

Public Class binder(Of T As Class, PROTECTOR)
    Inherits binder

    Private Shared ReadOnly is_protected As Boolean
#If DEBUG Then
    Private Shared _global_bind_callstack As String
#End If
    Private Shared _global As T
    Private _local As T

    Shared Sub New()
        is_protected = Not type_info(Of PROTECTOR).is_object
    End Sub

    Public Sub New()
    End Sub

    Public Sub New(ByVal i As T)
        set_local(i)
    End Sub

    Public Sub set_local(ByVal i As T)
        _local = i
    End Sub

    Public Shared Sub set_global(ByVal i As T)
        If Not is_suppressed.rebind_global_value() Then
#If DEBUG Then
            assert(_global Is Nothing OrElse i Is Nothing,
                   "rebind ",
                   GetType(T).FullName(),
                   " with protector ",
                   GetType(PROTECTOR).Name(),
                   ", last binding callstack ",
                   _global_bind_callstack)
#Else
            assert(_global Is Nothing OrElse i Is Nothing)
#End If
        End If
        _global = i
#If DEBUG Then
        If i Is Nothing Then
            _global_bind_callstack = Nothing
        Else
            _global_bind_callstack = callstack()
        End If
#End If
    End Sub

    Public Shared Function [global]() As T
        If _global Is Nothing AndAlso is_protected Then
            Return binder(Of T).global()
        Else
            Return _global
        End If
    End Function

    Public Function local() As T
        Return _local
    End Function

    Public Shared Function has_global() As Boolean
        Return Not [global]() Is Nothing
    End Function

    ' TODO: Remove, use has_global().
    Public Shared Function has_global_value() As Boolean
        Return has_global()
    End Function

    Public Function has_local() As Boolean
        Return Not local() Is Nothing
    End Function

    ' TODO: Remove, use has_local().
    Public Function has_local_value() As Boolean
        Return has_local()
    End Function

    Public Function [get]() As T
        If has_local_value() Then
            Return local()
        Else
            Return [global]()
        End If
    End Function

    Public Shared Operator +(ByVal this As binder(Of T, PROTECTOR)) As T
        If this Is Nothing Then
            Return [global]()
        Else
            Return this.get()
        End If
    End Operator
End Class
