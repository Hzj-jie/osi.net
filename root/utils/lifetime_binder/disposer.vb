
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.connector

Public Module _disposer
    Public Function regional_action(ByVal start As Action,
                                    ByVal [end] As Action) As disposer(Of Int32)
        assert(Not start Is Nothing)
        assert(Not [end] Is Nothing)
        Return make_disposer(0, start, Sub(x) [end]())
    End Function

    Public Function regional_atomic_bool(ByVal i As atomic_bool) As disposer(Of Int32)
        assert(Not i Is Nothing)
        Return make_disposer(0, Sub() i.inc(), Sub(x) i.dec())
    End Function
End Module
