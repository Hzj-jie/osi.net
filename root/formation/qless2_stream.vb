
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template

Public Class qless2_bytes_stream
    Inherits qless2_bytes_stream(Of _134217728)
End Class

Public Class qless2_bytes_stream(Of MAX_COUNT As _int64)
    Inherits qless2_stream(Of Byte, MAX_COUNT)
End Class

Public Class qless2_stream(Of T)
    Inherits qless2_stream(Of T, _134217728)
End Class

'partial thread-safe,
'the push is thread-safe, but the pop is not, so if multi-threads are poping together, it needs a lock
Public Class qless2_stream(Of T, MAX_COUNT As _int64)
    Public Shared ReadOnly max_size As Int64
    Private ReadOnly q As qless2(Of pointer(Of T()))
    Private len As Int64
    Private last() As T
    Private last_index As Int64

    Shared Sub New()
        max_size = +(alloc(Of MAX_COUNT)())
    End Sub

    Public Sub New()
        q = New qless2(Of pointer(Of T()))()
        len = 0
        last = Nothing
        last_index = 0
    End Sub

    Public Function size() As UInt64
        Dim r As Int64 = 0
        r = len
        Return If(r < 0, uint64_0, r)
    End Function

    Public Function empty() As Boolean
        Return size() = 0
    End Function

    Public Function pop(ByVal r() As T, ByVal offset As Int64, ByVal count As Int64) As Int64
        If offset < 0 OrElse count <= 0 OrElse array_size(r) < offset + count Then
            Return 0
        Else
            If last Is Nothing Then
                Dim p As pointer(Of T()) = Nothing
                If q.pop(p) Then
                    assert(Not p Is Nothing AndAlso Not isemptyarray(+p))
                    last = (+p)
                    last_index = 0
                Else
                    Return 0
                End If
            End If
            assert(last_index < array_size(last))
            Dim l As Int64 = 0
            l = min(array_size(last) - last_index, count - offset)
            memcpy(r, offset, last, last_index, l)
            last_index += l
            If array_size(last) = last_index Then
                last = Nothing
            End If
            assert(Interlocked.Add(len, -l) >= 0)
            Return l
        End If
    End Function

    Public Function pop(ByVal r() As T, Optional ByVal offset As Int64 = 0) As Int64
        Return pop(r, offset, array_size(r) - offset)
    End Function

    Public Function block_pop(ByVal len As Int64) As T()
        If len <= 0 Then
            Return Nothing
        Else
            Dim r() As T = Nothing
            ReDim r(len - 1)
            Dim i As Int64 = 0
            While i < len
                i += pop(r, i)
            End While
            Return r
        End If
    End Function

    Public Function push(ByVal d() As T) As Boolean
        If isemptyarray(d) OrElse
           len + array_size(d) >= max_size Then
            Return False
        Else
            Interlocked.Add(len, array_size(d))
            q.push(make_pointer(d))
            Return True
        End If
    End Function

    Public Sub clear()
        Dim r() As T = Nothing
        ReDim r(1024 - 1)
        While pop(r)
        End While
    End Sub
End Class
