
Imports osi.root.template

'just a set of classes to force using weak_event_handler
Public Class weak_event
    Inherits weak_reference_event(Of _false)
End Class

Public Class weak_event(Of T)
    Inherits weak_reference_event(Of _false, T)
End Class

Public Class weak_event(Of T1, T2)
    Inherits weak_reference_event(Of _false, T1, T2)
End Class

Public Class weak_event(Of T1, T2, T3)
    Inherits weak_reference_event(Of _false, T1, T2, T3)
End Class

Public Class once_weak_event
    Inherits weak_reference_event(Of _true)
End Class

Public Class once_weak_event(Of T)
    Inherits weak_reference_event(Of _true, T)
End Class

Public Class once_weak_event(Of T1, T2)
    Inherits weak_reference_event(Of _true, T1, T2)
End Class

Public Class once_weak_event(Of T1, T2, T3)
    Inherits weak_reference_event(Of _true, T1, T2, T3)
End Class

Public Class weak_reference_event(Of _ONCE As _boolean)
    Inherits action_event(Of _ONCE)

    Public Shadows Function attach(Of T)(ByVal v As T, ByVal a As Action) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T)(v, a))
    End Function

    Public Shadows Function attach(Of T)(ByVal v As T, ByVal a As Action(Of T)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T)(v, a))
    End Function
End Class

Public Class weak_reference_event(Of _ONCE As _boolean, T2)
    Inherits action_event(Of _ONCE, T2)

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T2)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2)(v, a))
    End Function

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T1, T2)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2)(v, a))
    End Function
End Class

Public Class weak_reference_event(Of _ONCE As _boolean, T2, T3)
    Inherits action_event(Of _ONCE, T2, T3)

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T2, T3)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2, T3)(v, a))
    End Function

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T1, T2, T3)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2, T3)(v, a))
    End Function
End Class

Public Class weak_reference_event(Of _ONCE As _boolean, T2, T3, T4)
    Inherits action_event(Of _ONCE, T2, T3, T4)

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T2, T3, T4)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2, T3, T4)(v, a))
    End Function

    Public Shadows Function attach(Of T1)(ByVal v As T1, ByVal a As Action(Of T1, T2, T3, T4)) As Boolean
        Return MyBase.attach(New weak_event_handler(Of T1, T2, T3, T4)(v, a))
    End Function
End Class
