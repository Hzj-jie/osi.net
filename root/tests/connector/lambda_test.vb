
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class lambda_test
    Inherits [case]

    Private Class C
        Public i As Int32
        Public result As ternary

        Public Sub New()
            i = 0
            result = ternary.unknown
        End Sub

        Public Sub [set](ByVal i As Int32)
            [set](Sub()
                      Me.i = i
                      Me.result = If(i > 100, ternary.true, ternary.false)
                  End Sub)
        End Sub

        Public Sub [set](ByVal c As Action)
            c()
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 10
            Dim x As Int32 = 0
            x = rnd_int()
            Dim c As C = Nothing
            c = New C()
            If rnd_bool() Then
                c.set(x)
            Else
                c.set(Sub()
                          c.i = x
                          c.result = If(x > 100, ternary.true, ternary.false)
                      End Sub)
            End If
            assertion.equal(c.i, x)
            assertion.is_false(c.result.unknown_())
            assertion.equal(c.result.true_(), x > 100)
        Next
        Return True
    End Function
End Class
