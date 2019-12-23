
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class map_reverse_test
    <test>
    Private Shared Sub normal()
        assertion.equal(map.emplace_of(emplace_make_pair("abc", 1),
                                       emplace_make_pair("bcd", 2)).reverse(),
                        map.emplace_of(emplace_make_pair(1, "abc"),
                                       emplace_make_pair(2, "bcd")))
    End Sub

    <test>
    Private Shared Sub allow_unique_values()
        assertion.is_true(map.emplace_of(emplace_make_pair("abc", 1),
                                         emplace_make_pair("bcd", 2)).reverse(Nothing))
    End Sub

    <test>
    Private Shared Sub disallow_dup_values()
        assertion.is_false(map.emplace_of(emplace_make_pair("abc", 1),
                                          emplace_make_pair("bcd", 1)).reverse(Nothing))
    End Sub

    Private Sub New()
    End Sub
End Class
