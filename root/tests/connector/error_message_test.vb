
Imports osi.root.connector
Imports osi.root.utt

Public Class error_message_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const s1 As String = "first, "
        Const s2 As String = "second, "
        Const s3 As String = "third, "
        Const s4 As String = "fourth, "
        Dim a() As Object = Nothing
        ReDim a(2)
        a(0) = s1
        a(1) = New String() {s2, s3}
        a(2) = s4

        Dim s As String = Nothing
        s = error_message.merge(a)
        assertion.equal(s, strcat(s1, s2, s3, s4))
        Return True
    End Function
End Class
