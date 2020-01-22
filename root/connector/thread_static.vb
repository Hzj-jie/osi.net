
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.delegates

'For mono debug purpose, the perf is a little bit worse than .net framework TheadStatic attribute.
' Mono cannot work well with thread-static primary types. But this class uses a set function to make sure even for
' primary types, using this class in Mono is safe.
Public NotInheritable Class thread_static(Of T)
    Private ReadOnly slot() As T

    Public Sub New(ByVal size As Int32)
        assert(size > 0)
        ReDim slot(size - 1)
    End Sub

    Public Sub New()
        Me.New(constants.thread_static_default_slot_size)
    End Sub

    Private Function current_slot_id(ByRef o As UInt32) As Boolean
        Dim i As Int32 = 0
        i = Thread.CurrentThread().ManagedThreadId() - 1
        assert(i >= 0)
        o = CUInt(i)
        Return o < array_size(slot)
    End Function

    Public Function [get](ByRef o As T) As Boolean
        Dim tid As UInt32 = 0
        If Not current_slot_id(tid) Then
            Return False
        End If
        o = slot(CInt(tid))
        Return True
    End Function

    Public Function [get]() As T
        Dim o As T = Nothing
        assert([get](o))
        Return o
    End Function

    Public Function [set](ByVal i As T) As Boolean
        Dim tid As UInt32 = 0
        If Not current_slot_id(tid) Then
            Return False
        End If
        slot(CInt(tid)) = i
        Return True
    End Function

    Public Function or_set(ByVal i As T, ByVal validate As Func(Of T, Boolean), ByRef o As T) As Boolean
        assert(Not i Is Nothing)
        Return or_set(func_t.of(i), validate, o)
    End Function

    Public Function or_set(ByVal i As T, ByVal validate As Func(Of T, Boolean)) As T
        Dim o As T = Nothing
        assert(or_set(i, validate, o))
        Return o
    End Function

    Public Function or_set(ByVal i As Func(Of T), validate As Func(Of T, Boolean), ByRef o As T) As Boolean
        assert(Not i Is Nothing)
        Dim tid As UInt32 = 0
        If Not current_slot_id(tid) Then
            Return False
        End If
        If Not validate(slot(CInt(tid))) Then
            slot(CInt(tid)) = i()
        End If
        o = slot(CInt(tid))
        Return True
    End Function

    Public Function or_set(ByVal i As Func(Of T), ByVal validate As Func(Of T, Boolean)) As T
        Dim o As T = Nothing
        assert(or_set(i, validate, o))
        Return o
    End Function

    Public Property at() As T
        Get
            Return [get]()
        End Get
        Set(ByVal value As T)
            assert([set](value))
        End Set
    End Property

    Public Sub clear()
        memclr(slot)
    End Sub

    Public Shared Operator +(ByVal this As thread_static(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.get()
    End Operator
End Class
