
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.connector

Public Module _disposer
    Public Function scoped_action(ByVal start As Action,
                                    ByVal [end] As Action) As disposer
        assert(Not start Is Nothing)
        assert(Not [end] Is Nothing)
        start()
        Return New disposer([end])
    End Function

    Public Function scoped_atomic_bool(ByVal i As atomic_bool) As disposer
        assert(Not i Is Nothing)
        Return scoped_action(AddressOf i.inc, AddressOf i.dec)
    End Function

    Public Function defer(ByVal [end] As Action) As disposer
        assert(Not [end] Is Nothing)
        Return New disposer([end])
    End Function
End Module
