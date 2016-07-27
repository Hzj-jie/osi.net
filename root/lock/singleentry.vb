
Imports System.Threading
Imports osi.root.connector

Public Structure singleentry
    Private Const free As Int32 = 0
    Private Const inuse As Int32 = 1
    Private state As Int32

    Shared Sub New()
        assert(free = DirectCast(Nothing, Int32))
    End Sub

    Public Function in_use() As Boolean
        Return state = inuse
    End Function

    Public Function not_in_use() As Boolean
        Return state = free
    End Function

    Public Function mark_in_use() As Boolean
        Return atomic.compare_exchange(state, inuse, free)
    End Function

    Public Sub release()
        assert(in_use())
        mark_not_in_use()
    End Sub

    Public Sub mark_not_in_use()
        atomic.eva(state, free)
    End Sub
End Structure
