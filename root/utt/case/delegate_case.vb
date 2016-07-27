
Imports osi.root.connector
Imports osi.root.delegates

Public Class delegate_case
    Inherits [case]

    Private ReadOnly r As Func(Of Boolean)

    Public Sub New(ByVal v As Action)
        assert(Not v Is Nothing)
        r = Function() As Boolean
                v()
                Return True
            End Function
    End Sub

    Public Sub New(ByVal f As Func(Of Boolean))
        assert(Not f Is Nothing)
        r = f
    End Sub

    Public Overrides Function run() As Boolean
        Return r()
    End Function
End Class
