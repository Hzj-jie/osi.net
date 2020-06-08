
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class stream_sort_test
    <test>
    Private Shared Sub ascending()
        assertion.array_equal(streams.of(2, 31, 3, 12, 3, 5, 2).
                                      sort().
                                      to_array(),
                              {2, 2, 3, 3, 5, 12, 31})
    End Sub

    <test>
    Private Shared Sub descending()
        assertion.array_equal(streams.of(2, 31, 3, 12, 3, 5, 2).
                                      sort(Function(ByVal i As Int32, ByVal j As Int32) As Int32
                                               Return j - i
                                           End Function).
                                      to_array(),
                              {31, 12, 5, 3, 3, 2, 2})
    End Sub

    Private Sub New()
    End Sub
End Class
