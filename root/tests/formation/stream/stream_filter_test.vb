
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

    Private Sub New()
    End Sub
End Class
