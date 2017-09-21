
Imports osi.root.template
Imports osi.root.utt

Public Class commander_manual_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New commander_case(Of _false, _false, _false, _true, _1)())
    End Sub
End Class
