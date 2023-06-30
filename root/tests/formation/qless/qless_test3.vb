
Imports osi.root.constants
Imports osi.root.utt

Public Class qless_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New qless_case3())
    End Sub
End Class

Public Class qless2_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New qless2_case3())
    End Sub
End Class

Public Class heapless_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New heapless_case3())
    End Sub
End Class

Public Class slimqless2_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New slimqless2_case3())
    End Sub
End Class

Public Class slimheapless_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New slimheapless_case3())
    End Sub
End Class

Public Class qless_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New qless_case3(max_int64, 32))
    End Sub
End Class

Public Class qless2_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New qless2_case3(max_int64, 32))
    End Sub
End Class

Public Class heapless_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New heapless_case3(max_int64, 32))
    End Sub
End Class

Public Class slimqless2_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New slimqless2_case3(max_int64, 32))
    End Sub
End Class

Public Class slimheapless_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New slimheapless_case3(max_int64, 32))
    End Sub
End Class

#If 0 Then
Public Class cycle_test3
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New cycle_1024_128_case3())
    End Sub
End Class

Public Class cycle_manual_test3
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New cycle_1024_128_case3(max_int64, 32))
    End Sub
End Class
#End If
