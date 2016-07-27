
Imports osi.root.connector
Imports osi.root.utt

Public Class heap_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New heap_case(), round())
    End Sub
End Class
