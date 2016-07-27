
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.template

Public Class cycle(Of T)
    Private Structure container
        Private v As T
        Private hv As singleentry
        Private written As singleentry
        Private read As singleentry

        Shared Sub New()
            Dim s As singleentry
            assert(s.not_in_use())
        End Sub

        Public Function [set](ByVal v As T) As Boolean
            If hv.mark_in_use() Then
                wait_when(Function(ByRef x As singleentry) As Boolean
                              Return x.in_use()
                          End Function,
                          written)
                Me.v = v
                assert(written.mark_in_use())
                If read.in_use() Then
                    read.release()
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Public Function [get](ByRef o As T) As Boolean
            If hv.in_use() AndAlso read.mark_in_use() Then
                wait_when(Function(ByRef x As singleentry) As Boolean
                              Return x.not_in_use()
                          End Function,
                          written)
                o = Me.v
                Me.v = Nothing
                written.release()
                hv.release()
                Return True
            Else
                Return False
            End If
        End Function
    End Structure

    Private ReadOnly a() As container
    Private ReadOnly try_times As Int64
    Private ReadOnly rnd_once As Boolean
    Private ReadOnly s As atomic_int

    Public Sub New(ByVal size As Int64, Optional ByVal try_times As Int64 = 0)
        assert(size > 0)
        ReDim a(size - 1)
        If try_times <= 0 Then
            Me.try_times = size
        Else
            Me.try_times = try_times
        End If
        rnd_once = (Me.try_times >= (size >> 1))
        s = New atomic_int()
    End Sub

    Public Function emplace(ByVal v As T) As Boolean
        If full() Then
            Return False
        Else
            If rnd_once Then
                Dim p As Int64 = 0
                p = rnd_int64(0, array_size(a))
                For i As Int64 = 0 To try_times - 1
                    If a(p).set(v) Then
                        Return True
                    Else
                        p += 1
                        If p = array_size(a) Then
                            p = 0
                        End If
                    End If
                Next
            Else
                For i As Int64 = 0 To try_times - 1
                    If a(rnd_int64(0, array_size(a))).set(v) Then
                        s.increment()
                        Return True
                    End If
                Next
            End If
            Return False
        End If
    End Function

    Public Function push(ByVal v As T) As Boolean
        Return emplace(copy_no_error(v))
    End Function

    Public Function pop(ByRef o As T) As Boolean
        If empty() Then
            Return False
        Else
            Dim p As Int64 = 0
            p = rnd_int64(0, array_size(a))
            For i As Int64 = 0 To array_size(a) - 1
                If a(p).get(o) Then
                    Return True
                Else
                    p += 1
                    If p = array_size(a) Then
                        p = 0
                    End If
                End If
            Next
            Return False
        End If
    End Function

    Public Function size() As Int64
        Dim v As Int64 = 0
        v = s.get()
        Return If(v < 0, 0, v)
    End Function

    Public Function empty() As Boolean
        Return s.get() <= 0
    End Function

    Public Function full() As Boolean
        Return s.get() >= array_size(a)
    End Function

    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub
End Class

Public Class cycle(Of T, SIZE As _int64, RETRY_TIMES As _int64)
    Inherits cycle(Of T)
    Private Shared ReadOnly default_size As Int64
    Private Shared ReadOnly default_retry_times As Int64

    Shared Sub New()
        default_size = +alloc(Of SIZE)()
    End Sub

    Public Sub New(ByVal size As Int64, ByVal retry_times As Int64)
        MyBase.New(size, retry_times)
    End Sub

    Public Sub New(ByVal size As Int64)
        Me.New(size, default_retry_times)
    End Sub

    Public Sub New()
        Me.New(default_size)
    End Sub
End Class

Public Class cycle_1024_128(Of T)
    Inherits cycle(Of T, _1024, _128)
End Class

#If 0 Then
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.template

Public Class cycle(Of T)
    Private Structure container
        Private v As T
        Private w As singleentry
        Private r As singleentry
        Private hv As singleentry

        Shared Sub New()
            Dim s As singleentry
            assert(s.not_in_use())
        End Sub

        Public Sub [set](ByVal v As T)
            wait_when(Function(ByRef x As singleentry) As Boolean
                          Return Not x.mark_in_use()
                      End Function,
                      w)
            wait_when(Function(ByRef x As singleentry) As Boolean
                          Return x.in_use()
                      End Function,
                      hv)
            Me.v = v
            assert(hv.mark_in_use())
            w.release()
        End Sub

        Public Function [get]() As T
            wait_when(Function(ByRef x As singleentry) As Boolean
                          Return Not x.mark_in_use()
                      End Function,
                      r)
            wait_when(Function(ByRef x As singleentry) As Boolean
                          Return x.not_in_use()
                      End Function,
                      hv)
            Dim o As T = Nothing
            o = v
            v = Nothing
            hv.release()
            r.release()
            Return o
        End Function
    End Structure

    Private ReadOnly a() As container
    Private ReadOnly f As atomic_uint64
    Private ReadOnly l As atomic_uint64

    Public Sub New(ByVal size As Int64)
        ReDim a(size - 1)
        f = New atomic_uint64()
        l = New atomic_uint64()
    End Sub

    Private Function array_index(ByVal i As UInt64) As Int32
        Return i Mod array_size(a)
    End Function

    Public Function emplace(ByVal v As T) As Boolean
        Dim p As UInt64 = 0
        p = l.increment() - 1
        If p >= f.get() + array_size(a) Then
            l.decrement()
            Return False
        Else
            a(array_index(p)).set(v)
            Return True
        End If
    End Function

    Public Function push(ByVal v As T) As Boolean
        Return emplace(copy_no_error(v))
    End Function

    Public Function pop(ByRef o As T) As Boolean
        Dim p As UInt64 = 0
        p = f.increment() - 1
        If p >= l.get() Then
            f.decrement()
            Return False
        Else
            o = a(array_index(p)).get()
            Return True
        End If
    End Function

    Public Function size() As Int64
        Dim f As UInt64 = 0
        Dim l As UInt64 = 0
        f = Me.f.get()
        l = Me.l.get()
        Return If(f >= l, 0, l - f)
    End Function

    Public Function empty() As Boolean
        Return f.get() >= l.get()
    End Function

    Public Function full() As Boolean
        Return l.get() >= f.get() + array_size(a)
    End Function

    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub
End Class
#End If