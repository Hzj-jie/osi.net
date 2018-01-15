
Option Explicit On
Option Infer Off
Option Strict On

Public MustInherit Class _boolean
    Inherits __do(Of Boolean)
End Class

Public NotInheritable Class _true
    Inherits _boolean

    Protected Overrides Function at() As Boolean
        Return True
    End Function
End Class

Public NotInheritable Class _false
    Inherits _boolean

    Protected Overrides Function at() As Boolean
        Return False
    End Function
End Class
