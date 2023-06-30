
Imports osi.root.constants

' A thread-safe integer which has a value in [min, max), increment (max - 1) returns min, and decrement min returns
' (max - 1).
Public Class range_int
    Private ReadOnly min As Int32
    Private ReadOnly max As Int32
    Private ReadOnly l As Object
    Private v As Int32

    Public Sub New(ByVal min As Int32, ByVal max As Int32)
        assert(min < max)
        assert(min > min_int32)
        assert(max < max_int32)
        Me.min = min
        Me.max = max
        Me.l = New Object()
        reset()
    End Sub

    Public Sub reset()
        v = min
    End Sub

    Public Function increment() As Int32
        SyncLock l
            v += 1
            If v = max Then
                v = min
            End If
            assert_in_range()
            Return v
        End SyncLock
    End Function

    Public Function decrement() As Int32
        SyncLock l
            v -= 1
            If v = min - 1 Then
                v = max - 1
            End If
            assert_in_range()
            Return v
        End SyncLock
    End Function

    Private Sub assert_in_range()
        assert(v >= min AndAlso v < max)
    End Sub
End Class
