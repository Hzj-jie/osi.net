
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_filter_test
    <test>
    Private Shared Sub filter_out_even()
        assertion.array_equal(streams.range(0, 10).
                                      filter(Function(ByVal i As Int32) As Boolean
                                                 Return i Mod 2 = 0
                                             End Function).
                                      to_array(),
                              {0, 2, 4, 6, 8})
    End Sub

    <test>
    Private Shared Sub filter_only_one()
        assertion.array_equal(streams.of(0).
                                      filter(Function(ByVal i As Int32) As Boolean
                                                 Return True
                                             End Function).
                                      to_array(),
                              {0})
    End Sub

    <test>
    Private Shared Sub filter_then_foreach()
        Dim r As Int32 = 0
        streams.of(1, 2, 3, 4, 5, 6).
                filter(Function(ByVal i As Int32) As Boolean
                           Return (i Mod 2) = 0
                       End Function).
                foreach(Sub(ByVal i As Int32)
                            r += i
                        End Sub)
        assertion.equal(r, 12)
    End Sub

    <test>
    Private Shared Sub filter_then_foreach_str()
        Dim r As New vector(Of String)()
        streams.of("aa", "ab", "bc", "ad", "bc").
                filter(Function(ByVal i As String) As Boolean
                           Return i.StartsWith("a")
                       End Function).
                foreach(Sub(ByVal i As String)
                            r.emplace_back(i)
                        End Sub)
        assertion.equal(r, vector.of("aa", "ab", "ad"))
    End Sub

    Private Sub New()
    End Sub
End Class
