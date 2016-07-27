
Imports osi.root.delegates
Imports osi.root.connector

Partial Public NotInheritable Class queue_runner
    Public Shared Function repeat(ByVal d As Action, ByRef f As Func(Of Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            f = Function() As Boolean
                    d()
                    Return True
                End Function
            Return True
        End If
    End Function

    Public Shared Function repeat(ByVal d As Action) As Func(Of Boolean)
        Dim f As Func(Of Boolean) = Nothing
        assert(repeat(d, f))
        Return f
    End Function
End Class