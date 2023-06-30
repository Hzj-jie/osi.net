
Imports osi.root.connector
Imports osi.root.utt

Public Class list_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New list_case(True), round())
    End Sub
End Class
