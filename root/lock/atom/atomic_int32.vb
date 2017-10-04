
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atomic_int32.vbp ----------
'so change atomic_int32.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atomic_int.vbp ----------
'so change atomic_int.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with atom_body.vbp ----------
'so change atom_body.vbp instead of this file


#Const GENERIC_TYPE = ("Int32" = "T")

Imports System.Threading
Imports osi.root.connector
#If GENERIC_TYPE Then
Imports osi.root.constants
#End If
Imports osi.root.delegates
Imports osi.root.lock.slimlock
Imports spinlock = osi.root.lock.slimlock.spinlock

Partial Public Class atomic_int32(Of LOCK_T As {islimlock, Structure})
    Private p As Int32
    Private l As LOCK_T

#If GENERIC_TYPE Then
    Shared Sub New()
        raise_error(error_type.performance,
                    "atomic_int32(Of LOCK_T) cannot use interlocked operations. Its performance is low.")
    End Sub
#End If

    Public Sub New()
    End Sub

    Public Sub New(ByVal i As Int32)
        [set](i)
    End Sub

    Public Function [get]() As Int32
#If GENERIC_TYPE Then
        Return l.locked(Function() p)
#Else
        Return atomic.read(p)
#End If
    End Function

    Public Sub [set](ByVal i As Int32)
#If GENERIC_TYPE Then
        l.locked(Sub() p = i)
#Else
        atomic.eva(p, i)
#End If
    End Sub

    Public Sub modify(ByVal d As void(Of Int32))
        If Not d Is Nothing Then
            l.locked(Sub() d(p))
        End If
    End Sub

    Public Function exchange(ByVal value As Int32) As Int32
#If GENERIC_TYPE Then
        Return l.locked(Function() As Int32
                            Dim r As Int32 = Nothing
                            r = p
                            p = value
                            Return r
                        End Function)
#Else
        Dim r As Int32 = Nothing
        r = Interlocked.Exchange(p, value)
        Thread.MemoryBarrier()
        Return r
#End If
    End Function

    Public Function compare_exchange(ByVal value As Int32, ByVal comparand As Int32) As Int32
#If GENERIC_TYPE Then
        Return l.locked(Function() As Int32
                            Dim r As Int32 = Nothing
                            r = p
                            If equal(p, comparand) Then
                                p = value
                            End If
                            Return r
                        End Function)
#Else
        Dim r As Int32 = Nothing
        r = Interlocked.CompareExchange(p, value, comparand)
        Thread.MemoryBarrier()
        Return r
#End If
    End Function

    Public Shared Operator +(ByVal this As atomic_int32(Of LOCK_T)) As Int32
        Return If(this Is Nothing, Nothing, this.get())
    End Operator
End Class


Public Class atomic_int32
    Inherits atomic_int32(Of spinlock)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal i As Int32)
        MyBase.New(i)
    End Sub
End Class

'finish atom_body.vbp --------

Partial Public Class atomic_int32(Of LOCK_T As {islimlock, Structure})
    Public Function increment() As Int32
        Dim r As Int32 = 0
        r = Interlocked.Increment(p)
        Thread.MemoryBarrier()
        Return r
    End Function

    Public Function decrement() As Int32
        Dim r As Int32 = 0
        r = Interlocked.Decrement(p)
        Thread.MemoryBarrier()
        Return r
    End Function

    Public Function add(ByVal i As Int32) As Int32
        Dim r As Int32 = 0
        r = Interlocked.Add(p, i)
        Thread.MemoryBarrier()
        Return r
    End Function
End Class
'finish atomic_int.vbp --------
'finish atomic_int32.vbp --------
