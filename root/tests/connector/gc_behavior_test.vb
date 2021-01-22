
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class gc_behavior_test
    Inherits [case]

    Private NotInheritable Class cc
        Public finialized As Boolean

        Public Sub New()
            finialized = False
        End Sub

        <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1821:RemoveEmptyFinalizers")>
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
            Me.b = direct_cast(Of String)(gc_behavior_test.b.Clone())
            Me.c = New Object()
            Me.d = direct_cast(Of Version)(gc_behavior_test.d.Clone())
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
        Return CInt(rnd_uint(uint32_0, CUInt(max_int32) + uint32_1))
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
        garbage_collector.repeat_collect()
        assertion.equal(+a, CInt(r))
        Return True
    End Function
End Class
