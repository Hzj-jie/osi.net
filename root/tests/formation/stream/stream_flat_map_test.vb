
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_flat_map_test
    <test>
    Private Shared Sub case1()
        Dim v As vector(Of String) = Nothing
        v = New vector(Of String)()
        For i As Int32 = 0 To 100
            v.emplace_back(guid_str())
        Next
        assertion.equal(v.stream().
                          flat_map(Function(s) streams.of(s.c_str())).
                          collect_by(stream(Of Char).collectors.to_str("")).
                          ToString(),
                        v.str(""))
    End Sub

    Private Sub New()
    End Sub
End Class
