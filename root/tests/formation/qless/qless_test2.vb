
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation

Public Class qless_test2
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New qless_case2())
    End Sub
End Class

Public Class qless2_test2
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New qless2_case2())
    End Sub
End Class

Public Class heapless_test2
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New heapless_case2())
    End Sub
End Class

#If 0 Then
Public Class cycle_test2
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New cycle_1024_128_case2())
    End Sub
End Class
#End If
