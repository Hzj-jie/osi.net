
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class thread_boost
    Inherits priority

    Public Sub New()
        MyBase.New(nice.thread_boost)
    End Sub
End Class

Public NotInheritable Class thread_moderate
    Inherits priority

    Public Sub New()
        MyBase.New(nice.thread_moderate)
    End Sub
End Class

Public NotInheritable Class thread_lazy
    Inherits priority

    Public Sub New()
        MyBase.New(nice.thread_lazy)
    End Sub
End Class
