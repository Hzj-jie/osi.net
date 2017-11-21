﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs

Public Class atomic
    Private Shared ReadOnly x86 As Boolean = False
    Private Shared ReadOnly amd64 As Boolean = False
    Private Shared ReadOnly create_auto_reset_event_true As Func(Of AutoResetEvent)
    Private Shared ReadOnly create_auto_reset_event_false As Func(Of AutoResetEvent)
    Private Shared ReadOnly create_manual_reset_event_true As Func(Of ManualResetEvent)
    Private Shared ReadOnly create_manual_reset_event_false As Func(Of ManualResetEvent)
    Private Shared ReadOnly destroy_auto_reset_event As Action(Of AutoResetEvent)
    Private Shared ReadOnly destroy_manual_reset_event As Action(Of ManualResetEvent)

    Shared Sub New()
        x86 = x32_cpu
        amd64 = x64_cpu

        create_auto_reset_event_true = Function() As AutoResetEvent
                                           Return New AutoResetEvent(True)
                                       End Function
        create_auto_reset_event_false = Function() As AutoResetEvent
                                            Return New AutoResetEvent(False)
                                        End Function
        create_manual_reset_event_true = Function() As ManualResetEvent
                                             Return New ManualResetEvent(True)
                                         End Function
        create_manual_reset_event_false = Function() As ManualResetEvent
                                              Return New ManualResetEvent(False)
                                          End Function
        destroy_auto_reset_event = Sub(x As AutoResetEvent)
                                       assert(Not x Is Nothing)
                                       x.Close()
                                   End Sub
        destroy_manual_reset_event = Sub(x As ManualResetEvent)
                                         assert(Not x Is Nothing)
                                         x.Close()
                                     End Sub
    End Sub

    Public Shared Sub eva(ByRef i As Single, ByVal j As Single)
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(ByRef i As Double, ByVal j As Double)
        'based on atomTests.doubleAtomTest, double is using FPU, so it's safe to use VolatileWrite
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(ByRef i As Int32, ByVal j As Int32)
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(ByRef i As Int64, ByVal j As Int64)
        If amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(ByRef i As IntPtr, ByVal j As IntPtr)
        Thread.VolatileWrite(i, j)
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(ByRef i As Object, ByVal j As Object)
        Interlocked.Exchange(i, j)
        Thread.MemoryBarrier()
    End Sub

    Public Shared Sub eva(Of T As Class)(ByRef i As T, ByVal j As T)
        Interlocked.Exchange(i, j)
        Thread.MemoryBarrier()
    End Sub

    Public Shared Function read(ByRef i As Int64) As Int64
        Thread.MemoryBarrier()
        If amd64 Then
            Return Thread.VolatileRead(i)
        Else
            Return Interlocked.Read(i)
        End If
    End Function

    Public Shared Function read(ByRef i As Int32) As Int32
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    Public Shared Function read(ByRef i As Double) As Double
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    Public Shared Function read(ByRef i As Object) As Object
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    Public Shared Function read(Of T As Class)(ByRef i As T) As T
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return direct_cast(Of T)(Thread.VolatileRead(unref(i)))
    End Function

    Public Shared Function compare_exchange(ByRef i As Int32, ByVal v As Int32, ByVal cmp As Int32) As Boolean
        Thread.MemoryBarrier()
        If i = cmp Then
            Return Interlocked.CompareExchange(i, v, cmp) = cmp
        Else
            Return False
        End If
    End Function

    Public Shared Function clear_if_not_nothing(Of T As Class)(ByRef i As T, Optional ByRef o As T = Nothing) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Thread.MemoryBarrier()
            o = i
            If o Is Nothing Then
                Return False
            Else
                Return Interlocked.CompareExchange(i, Nothing, o) Is o
            End If
        End If
    End Function

    Public Shared Function set_if_nothing(Of T As Class)(ByRef i As T,
                                                         ByVal n As T,
                                                         Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If i Is Nothing AndAlso Not n Is Nothing Then
            Thread.MemoryBarrier()
            If Interlocked.CompareExchange(i, n, Nothing) Is Nothing AndAlso
               assert(Not i Is Nothing) Then
                Return True
            Else
                If Not destroy Is Nothing Then
                    destroy(n)
                End If
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function create_if_nothing(Of T As Class)(ByRef i As T,
                                                            ByVal ctor As Func(Of T),
                                                            Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If i Is Nothing Then
            Dim v As T = Nothing
            v = ctor()
            Thread.MemoryBarrier()
            If Interlocked.CompareExchange(i, v, Nothing) Is Nothing AndAlso
                   assert(Not i Is Nothing) Then
                Return True
            Else
                If Not destroy Is Nothing Then
                    destroy(v)
                End If
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function create_if_nothing(Of T As {Class, New}) _
                                            (ByRef i As T,
                                             Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If i Is Nothing Then
            Return create_if_nothing(i, Function() New T(), destroy)
        Else
            Return False
        End If
    End Function

    Public Shared Function create_if_nothing(ByRef i As AutoResetEvent,
                                             ByVal init_state As Boolean) As Boolean
        Return create_if_nothing(i,
                                 If(init_state,
                                    create_auto_reset_event_true,
                                    create_auto_reset_event_false),
                                 destroy_auto_reset_event)
    End Function

    Public Shared Function create_if_nothing(ByRef i As ManualResetEvent,
                                             ByVal init_state As Boolean) As Boolean
        Return create_if_nothing(i,
                                 If(init_state,
                                    create_manual_reset_event_true,
                                    create_manual_reset_event_false),
                                 destroy_manual_reset_event)
    End Function
End Class
