
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils

Public Module _ithreadpool_extension
    <Extension()> Public Sub queue_job(Of T)(ByVal p As ithreadpool, ByVal d As void(Of T), ByVal i As T)
        assert(Not d Is Nothing)
        p.queue_job(Sub(o) d(o), i)
    End Sub

    <Extension()> Public Sub queue_job(Of T)(ByVal p As ithreadpool, ByVal d As Action(Of T), ByVal i As T)
        assert(Not p Is Nothing)
        assert(Not d Is Nothing)
        p.queue_job(Sub() d(i))
    End Sub
End Module
