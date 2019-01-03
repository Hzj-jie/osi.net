
Imports System.Net
Imports osi.root.connector
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
        For i As Int32 = 0 To array_size(match_cases) - 1
            assertion.is_true(New IPEndPoint(IPAddress.Parse(match_cases(i).first), 1000).match_endpoint(
                        New IPEndPoint(IPAddress.Parse(match_cases(i).second), 1000)))
            assertion.is_true(New IPEndPoint(IPAddress.Parse(match_cases(i).first), 1000).match_endpoint(
                        New IPEndPoint(IPAddress.Parse(match_cases(i).second), 0)))
        Next
        For i As Int32 = 0 To array_size(unmatch_cases) - 1
            assertion.is_false(New IPEndPoint(IPAddress.Parse(unmatch_cases(i).first), 1000).match_endpoint(
                         New IPEndPoint(IPAddress.Parse(unmatch_cases(i).second), 1000)))
            assertion.is_false(New IPEndPoint(IPAddress.Parse(unmatch_cases(i).first), 1000).match_endpoint(
                         New IPEndPoint(IPAddress.Parse(unmatch_cases(i).second), 0)))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return match_endpoint_test()
    End Function
End Class
