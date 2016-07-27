
Imports System.Threading

'For mono debug purpose, the perf is a little bit worse than .net framework TheadStatic attribute.
' Mono cannot work well with thread-static primary types. But this class uses a set function to make sure even for
' primary types, using this class in Mono is safe.
Public Class thread_static(Of T)
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
        If current_slot_id(tid) Then
            o = slot(tid)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function [get]() As T
        Dim o As T = Nothing
        assert([get](o))
        Return o
    End Function

    Public Function [set](ByVal i As T) As Boolean
        Dim tid As UInt32 = 0
        If current_slot_id(tid) Then
            slot(tid) = i
            Return True
        Else
            Return False
        End If
    End Function

    Public Property at() As T
        Get
            Return [get]()
        End Get
        Set(ByVal value As T)
            assert([set](value))
        End Set
    End Property

    Public Shared Operator +(ByVal this As thread_static(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.get()
        End If
    End Operator
End Class
