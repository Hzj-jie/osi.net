
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock

Public Module _disposer_ext
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

    ' TODO: Remove and use connector.deferring.to
    Public Function defer(ByVal [end] As Action) As disposer
        assert(Not [end] Is Nothing)
        Return New disposer([end])
    End Function
End Module
