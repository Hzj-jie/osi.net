
Public Class realtime
    Inherits priority

    Public Sub New()
        MyBase.New(nice.realtime)
    End Sub
End Class

Public Class boost
    Inherits priority

    Public Sub New()
        MyBase.New(nice.boost)
    End Sub
End Class

Public Class lazy
    Inherits priority

    Public Sub New()
        MyBase.New(nice.lazy)
    End Sub
End Class

Public Class moderate
    Inherits priority

    Public Sub New()
        MyBase.New(nice.moderate)
    End Sub
End Class

Public Class process_realtime
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_realtime)
    End Sub
End Class

Public Class process_boost
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_boost)
    End Sub
End Class

Public Class process_lazy
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_lazy)
    End Sub
End Class

Public Class process_moderate
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_moderate)
    End Sub
End Class