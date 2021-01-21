
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.connector

'the queue will fail if the count of elements is over _MAX_SIZE
'so only for a critical performance requirement
Public Structure fixed_queue(Of T, _MAX_SIZE As _int64)
    Private Shared ReadOnly MAX_SIZE_D_1 As Int32 = CInt(+(alloc(Of _MAX_SIZE)()))
    Private Shared ReadOnly MAX_SIZE As Int32 = MAX_SIZE_D_1 + 1

    Private q() As T
    Private start As Int32
    Private last As Int32

    Private Sub create()
        If q Is Nothing Then
            ReDim q(MAX_SIZE_D_1)
            assert(Not q Is Nothing)
        End If
    End Sub

    Private Sub inc(ByRef i As Int32)
        i += 1
        If i = MAX_SIZE Then
            i = 0
        End If
    End Sub

    Public Sub push(ByVal i As T)
        emplace(copy_no_error(i))
    End Sub

    Public Sub emplace(ByVal i As T)
        create()
        assert(Not full())
        q(last) = i
        inc(last)
    End Sub

    Public Function size() As Int64
        Dim rtn As Int32 = 0
        rtn = last - start
        If rtn < 0 Then
            rtn += MAX_SIZE
        End If
        Return rtn
    End Function

    Public Function full() As Boolean
        Return size() = MAX_SIZE_D_1
    End Function

    Public Function empty() As Boolean
        Return size() = 0
    End Function

    Public Function pop(ByRef o As T) As Boolean
        If empty() Then
            Return False
        Else
            o = q(start)
            q(start) = Nothing
            inc(start)
            Return True
        End If
    End Function

    Public Function pop() As T
        Dim r As T = Nothing
        assert(pop(r))
        Return r
    End Function

    Public Sub clear()
        start = 0
        last = 0
        arrays.clear(q)
    End Sub
End Structure
