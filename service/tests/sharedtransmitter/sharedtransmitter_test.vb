
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Partial Public Class sharedtransmitter_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(New outgoing_test(), New incoming_test(), New bidirectional_test())
    End Sub

    Public Class sharedtransmitter_outgoing_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New outgoing_test())
        End Sub
    End Class

    Public Class sharedtransmitter_incoming_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New incoming_test())
        End Sub
    End Class

    Public Class sharedtransmitter_bidirectional_test
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New bidirectional_test())
        End Sub
    End Class
End Class
