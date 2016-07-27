
Imports osi.root.constants

Public MustInherit Class _string
    Inherits __do(Of String)
End Class

Public Class _a
    Inherits _string

    Private Const r As String = character.a

    Protected Overrides Function at() As String
        Return r
    End Function
End Class

Public Class __A
    Inherits _string

    Private Const r As String = character._A

    Protected Overrides Function at() As String
        Return r
    End Function
End Class

Public Class _left_slash
    Inherits _string

    Private Const r As String = character.left_slash

    Protected Overrides Function at() As String
        Return r
    End Function
End Class

Public Class _dot
    Inherits _string

    Private Const r As String = character.dot

    Protected Overrides Function at() As String
        Return r
    End Function
End Class
