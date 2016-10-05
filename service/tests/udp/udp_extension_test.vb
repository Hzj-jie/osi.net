
Imports System.Net
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.udp

Public Class udp_extension_test
    Inherits [case]

    Private Shared ReadOnly match_cases() As pair(Of String, String)
    Private Shared ReadOnly unmatch_cases() As pair(Of String, String)

    Shared Sub New()
        match_cases = {
            emplace_make_pair("10.0.0.1", "10.0.0.255"),
            emplace_make_pair("10.0.0.1", "10.255.255.255")
        }

        unmatch_cases = {
            emplace_make_pair("10.0.0.1", "10.0.0.254"),
            emplace_make_pair("10.0.0.1", "10.0.1.255")
        }
    End Sub

    Private Shared Function match_endpoint_test() As Boolean

    End Function

    Public Overrides Function run() As Boolean
        Return match_endpoint_test()
    End Function
End Class
