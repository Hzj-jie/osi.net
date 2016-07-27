
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