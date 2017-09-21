
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Partial Public Class shared_component_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(New outgoing_test(), New incoming_test(), New bidirectional_test())
    End Sub

    Public Class shared_component_outgoing_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New outgoing_test())
        End Sub
    End Class

    Public Class shared_component_incoming_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New incoming_test())
        End Sub
    End Class

    Public Class shared_component_bidirectional_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New bidirectional_test())
        End Sub
    End Class
End Class
