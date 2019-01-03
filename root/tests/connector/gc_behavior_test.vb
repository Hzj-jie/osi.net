
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.utt

Public Class gc_behavior_test
    Inherits [case]

    Private Class cc
        Public finialized As Boolean

        Public Sub New()
            finialized = False
        End Sub

        Protected Overrides Sub Finalize()
            finialized = True
            MyBase.Finalize()
        End Sub
    End Class

    Private Class tc
        Private ReadOnly a As UInt32
        Private ReadOnly b As String
        Private ReadOnly c As Object
        Private ReadOnly d As Version
        Private ReadOnly e As atomic_int
        Private ReadOnly f As cc

        Public Sub New(ByVal e As atomic_int)
            assert(Not e Is Nothing)
            Me.a = gc_behavior_test.a
            Me.b = gc_behavior_test.b.Clone()
            Me.c = New Object()
            Me.d = gc_behavior_test.d.Clone()
            Me.e = e
            Me.f = New cc()
        End Sub

        Protected Overrides Sub Finalize()
            assertion.equal(a, gc_behavior_test.a)
            assertion.is_not_null(b)
            assertion.equal(b, gc_behavior_test.b)
            assertion.is_not_null(c)
            assertion.is_not_null(d)
            assertion.equal(d, gc_behavior_test.d)
            If assertion.is_not_null(e) Then
                e.increment()
            End If
#If 0 Then
            ' There is way to make sure f has been finalized after this object.
            assertion.is_false(f.finialized)
            GC.KeepAlive(f)
#End If
            MyBase.Finalize()
        End Sub
    End Class

    Private Shared ReadOnly a As UInt32
    Private Shared ReadOnly b As String
    Private Shared ReadOnly d As Version

    Shared Sub New()
        a = rnd_uint()
        b = guid_str()
        d = New Version(rnd_uint_int(), rnd_uint_int(), rnd_uint_int(), rnd_uint_int())
    End Sub

    Private Shared Function rnd_uint_int() As Int32
        Return rnd_uint(uint32_0, CUInt(max_int32) + 1)
    End Function

    Public Overrides Function run() As Boolean
        Const r As UInt32 = 10000
        Dim a As atomic_int = Nothing
        a = New atomic_int()
        For i As UInt32 = 0 To r - uint32_1
            Dim x As tc = Nothing
            x = New tc(a)
            x = Nothing
        Next
        repeat_gc_collect()
        assertion.equal(+a, CInt(r))
        Return True
    End Function
End Class
