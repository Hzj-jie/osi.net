
Imports System.Threading
Imports osi.root.utt
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.envs
Imports osi.root.template

Friend Class qless2_stream_case
    Inherits [case]

    Private Const max_size As Int32 = 16777216
    Private ReadOnly s As qless2_bytes_stream
    Private ReadOnly popped() As Boolean
    Private pushed As Int32
    Private pop_tid As Int32

    Public Sub New(ByVal verify As Boolean)
        s = New qless2_bytes_stream()
        If verify Then
            ReDim popped(max_size - 1)
        End If
    End Sub

    Public Overrides Function finish() As Boolean
        s.clear()
        arrays.clear(popped)
        pushed = 0
        pop_tid = 0
        Return MyBase.finish()
    End Function

    Public Overrides Function run() As Boolean
        If Interlocked.CompareExchange(pop_tid, current_thread_id(), 0) = 0 Then
            While pushed < max_size OrElse Not s.empty()
                pop()
            End While
        Else
            While push()
            End While
        End If
        Return True
    End Function

    Private Function push() As Boolean
        If pushed < max_size Then
            Dim c As Int32 = 0
            c = rnd_int(0, 8) + 1
            Dim b() As Byte = Nothing
            ReDim b(c * sizeof_int32 - 1)
            For i As Int32 = 0 To c - 1
                Dim r As Int32 = 0
                r = Interlocked.Increment(pushed) - 1
                If r < max_size Then
                    assert(int32_bytes(r, b, i * sizeof_int32))
                Else
                    ReDim Preserve b(i * sizeof_int32 - 1)
                    Exit For
                End If
            Next
            If Not isemptyarray(b) Then
                assertion.is_true(s.push(b))
                Return True
            End If
        End If
        Return False
    End Function

    Private Function verify() As Boolean
        Return Not popped Is Nothing
    End Function

    Private Sub not_popped(ByVal b() As Byte)
        If verify() Then
            assertion.equal(array_size(b) Mod sizeof_int32, uint32_0)
            For i As Int32 = 0 To (array_size(b) \ sizeof_int32) - 1
                Dim j As Int32 = 0
                assertion.is_true(bytes_int32(b, j, i * sizeof_int32))
                If assertion.is_true(j < max_size AndAlso j >= 0) Then
                    assertion.is_false(popped(j), j)
                    popped(j) = True
                End If
            Next
        End If
    End Sub

    Private Sub pop()
        Dim b() As Byte = Nothing
        ReDim b((rnd_int(7, 16) + 1) * sizeof_int32 - 1)
        Dim p As Int64 = 0
        p = s.pop(b)
        assertion.equal(p Mod sizeof_int32, 0)
        If verify() AndAlso p < array_size(b) AndAlso p > 0 Then
            ReDim Preserve b(p - 1)
            not_popped(b)
        End If
    End Sub
End Class
