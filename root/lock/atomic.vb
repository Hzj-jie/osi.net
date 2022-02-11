
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Public NotInheritable Class atomic
    Private Shared ReadOnly x86 As Boolean = x32_cpu
    Private Shared ReadOnly amd64 As Boolean = x64_cpu
    Private Shared ReadOnly create_auto_reset_event_true As Func(Of AutoResetEvent) =
        Function() As AutoResetEvent
            Return New AutoResetEvent(True)
        End Function
    Private Shared ReadOnly create_auto_reset_event_false As Func(Of AutoResetEvent) =
        Function() As AutoResetEvent
            Return New AutoResetEvent(False)
        End Function
    Private Shared ReadOnly create_manual_reset_event_true As Func(Of ManualResetEvent) =
        Function() As ManualResetEvent
            Return New ManualResetEvent(True)
        End Function
    Private Shared ReadOnly create_manual_reset_event_false As Func(Of ManualResetEvent) =
        Function() As ManualResetEvent
            Return New ManualResetEvent(False)
        End Function
    Private Shared ReadOnly destroy_auto_reset_event As Action(Of AutoResetEvent) =
        Sub(ByVal x As AutoResetEvent)
            assert(Not x Is Nothing)
            x.Close()
        End Sub
    Private Shared ReadOnly destroy_manual_reset_event As Action(Of ManualResetEvent) =
        Sub(ByVal x As ManualResetEvent)
            assert(Not x Is Nothing)
            x.Close()
        End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As Single, ByVal j As Single)
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As Double, ByVal j As Double)
        'based on atomTests.doubleAtomTest, double is using FPU, so it's safe to use VolatileWrite
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As Int32, ByVal j As Int32)
        If x86 OrElse amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As Int64, ByVal j As Int64)
        If amd64 Then
            Thread.VolatileWrite(i, j)
        Else
            Interlocked.Exchange(i, j)
        End If
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As IntPtr, ByVal j As IntPtr)
        Thread.VolatileWrite(i, j)
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(ByRef i As Object, ByVal j As Object)
        Interlocked.Exchange(i, j)
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub eva(Of T As Class)(ByRef i As T, ByVal j As T)
        Interlocked.Exchange(i, j)
        Thread.MemoryBarrier()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function read(ByRef i As Int64) As Int64
        Thread.MemoryBarrier()
        If amd64 Then
            Return Thread.VolatileRead(i)
        End If
        Return Interlocked.Read(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function read(ByRef i As Int32) As Int32
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function read(ByRef i As Double) As Double
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function read(ByRef i As Object) As Object
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return Thread.VolatileRead(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function read(Of T As Class)(ByRef i As T) As T
        'no support in .net framework for cpu <32 bit
        Thread.MemoryBarrier()
        Return direct_cast(Of T)(Thread.VolatileRead(unref(i)))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function increment(ByRef i As Int32, ByVal max As UInt32) As Boolean
        Thread.MemoryBarrier()
        If Interlocked.Increment(i) > max Then
            Interlocked.Decrement(i)
            Return False
        End If
        Thread.MemoryBarrier()
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function increment(ByRef i As Int32, ByVal max As Int32) As Boolean
        If max < 0 Then
            Return False
        End If
        Return increment(i, CUInt(max))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function decrement(ByRef i As Int32, ByVal min As Int32) As Boolean
        Thread.MemoryBarrier()
        If Interlocked.Decrement(i) < min Then
            Interlocked.Increment(i)
            Return False
        End If
        Thread.MemoryBarrier()
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function compare_exchange(ByRef i As Int32, ByVal v As Int32, ByVal cmp As Int32) As Boolean
        Thread.MemoryBarrier()
        If i = cmp Then
            Return Interlocked.CompareExchange(i, v, cmp) = cmp
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function compare_exchange(Of T As Class)(ByRef i As T, ByVal v As T, ByVal cmp As T) As Boolean
        Thread.MemoryBarrier()
        If Object.ReferenceEquals(i, cmp) Then
            Return Object.ReferenceEquals(Interlocked.CompareExchange(i, v, cmp), cmp)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function clear_if_not_nothing(Of T As Class)(ByRef i As T, Optional ByRef o As T = Nothing) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Thread.MemoryBarrier()
        o = i
        If o Is Nothing Then
            Return False
        End If
        Return Interlocked.CompareExchange(i, Nothing, o) Is o
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function set_if_nothing(Of T As Class)(ByRef i As T,
                                                         ByVal n As T,
                                                         Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If Not i Is Nothing OrElse n Is Nothing Then
            Return False
        End If
        Thread.MemoryBarrier()
        If Interlocked.CompareExchange(i, n, Nothing) Is Nothing Then
            Return True
        End If
        If Not destroy Is Nothing Then
            destroy(n)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function create_if_nothing(Of T As Class)(ByRef i As T,
                                                            ByVal ctor As Func(Of T),
                                                            Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If Not i Is Nothing Then
            Return False
        End If
        Dim v As T = Nothing
        v = ctor()
        Thread.MemoryBarrier()
        If Interlocked.CompareExchange(i, v, Nothing) Is Nothing Then
            Return True
        End If
        If Not destroy Is Nothing Then
            destroy(v)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function create_if_nothing(Of T As {Class, New}) _
                                            (ByRef i As T,
                                             Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        If i Is Nothing Then
            Return create_if_nothing(i, Function() New T(), destroy)
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function create_if_nothing(ByRef i As AutoResetEvent,
                                             ByVal init_state As Boolean) As Boolean
        Return create_if_nothing(i,
                                 If(init_state,
                                    create_auto_reset_event_true,
                                    create_auto_reset_event_false),
                                 destroy_auto_reset_event)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function create_if_nothing(ByRef i As ManualResetEvent,
                                             ByVal init_state As Boolean) As Boolean
        Return create_if_nothing(i,
                                 If(init_state,
                                    create_manual_reset_event_true,
                                    create_manual_reset_event_false),
                                 destroy_manual_reset_event)
    End Function
End Class
