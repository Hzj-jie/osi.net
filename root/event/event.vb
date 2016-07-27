
Imports osi.root.template

Public Class [event]
    Inherits action_event(Of _false)
End Class

Public Class [event](Of T)
    Inherits action_event(Of _false, T)
End Class

Public Class [event](Of T1, T2)
    Inherits action_event(Of _false, T1, T2)
End Class

Public Class [event](Of T1, T2, T3)
    Inherits action_event(Of _false, T1, T2, T3)
End Class

Public Class once_event
    Inherits action_event(Of _true)
End Class

Public Class once_event(Of T)
    Inherits action_event(Of _true, T)
End Class

Public Class once_event(Of T1, T2)
    Inherits action_event(Of _true, T1, T2)
End Class

Public Class once_event(Of T1, T2, T3)
    Inherits action_event(Of _true, T1, T2, T3)
End Class
