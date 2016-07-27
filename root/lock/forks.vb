
Imports osi.root.connector
Imports System.Threading

Public Structure forks
    Private Const f As Int32 = 0
    Private state As Int32

    Shared Sub New()
        assert(f = DirectCast(Nothing, Int32))
    End Sub

    Public Function free() As Boolean
        Return state = f
    End Function

    Public Function not_free() As Boolean
        Return state <> f
    End Function

    Public Function mark_as(ByVal s As Int32) As Boolean
        assert(s <> f)
        Return Interlocked.CompareExchange(state, s, f) = f
    End Function

    Public Sub mark_free()
        atomic.eva(state, f)
    End Sub

    Public Sub release()
        assert(not_free())
        mark_free()
    End Sub
End Structure
