
Imports osi.root.formation

Public Class mock_dev_int
    Inherits mock_dev_T(Of Int32)

    Public Sub New()
        MyBase.New()
    End Sub

    Private Sub New(ByVal send_q As qless2(Of Int32),
                    ByVal receive_q As qless2(Of Int32))
        MyBase.New(send_q, receive_q)
    End Sub

    Public Shadows Function the_other_end() As mock_dev_int
        Return MyBase.the_other_end(Function(x, y) New mock_dev_int(x, y))
    End Function

    Protected Overrides Function consistent(ByVal x As Int32, ByVal y As Int32) As Boolean
        Return x = y
    End Function
End Class
