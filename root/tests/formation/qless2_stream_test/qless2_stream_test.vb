
Imports osi.root.utt
Imports osi.root.connector

Public Class qless2_stream_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(New qless2_stream_case(True))
    End Sub
End Class
