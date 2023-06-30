
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class realtime
    Inherits priority

    Public Sub New()
        MyBase.New(nice.realtime)
    End Sub
End Class

Public NotInheritable Class boost
    Inherits priority

    Public Sub New()
        MyBase.New(nice.boost)
    End Sub
End Class

Public NotInheritable Class lazy
    Inherits priority

    Public Sub New()
        MyBase.New(nice.lazy)
    End Sub
End Class

Public NotInheritable Class moderate
    Inherits priority

    Public Sub New()
        MyBase.New(nice.moderate)
    End Sub
End Class

Public NotInheritable Class process_realtime
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_realtime)
    End Sub
End Class

Public NotInheritable Class process_boost
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_boost)
    End Sub
End Class

Public NotInheritable Class process_lazy
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_lazy)
    End Sub
End Class

Public NotInheritable Class process_moderate
    Inherits priority

    Public Sub New()
        MyBase.New(nice.process_moderate)
    End Sub
End Class