
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class cctor_delegator
    Public Sub New(ByVal v As Action)
        assert(Not v Is Nothing)
        v()
    End Sub

    Public Sub init()
    End Sub
End Class
