
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.connector

Public Module _disposer
    Public Function regional_action(ByVal start As Action,
                                    ByVal [end] As Action) As disposer
        assert(Not start Is Nothing)
        assert(Not [end] Is Nothing)
        start()
        Return New disposer([end])
    End Function

    Public Function regional_atomic_bool(ByVal i As atomic_bool) As disposer
        assert(Not i Is Nothing)
        Return regional_action(AddressOf i.inc, AddressOf i.dec)
    End Function
End Module
