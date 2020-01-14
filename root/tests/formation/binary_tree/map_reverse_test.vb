
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
        assertion.equal(map.emplace_of(pair.emplace_of("abc", 1),
                                       pair.emplace_of("bcd", 2)).reverse(),
                        map.emplace_of(pair.emplace_of(1, "abc"),
                                       pair.emplace_of(2, "bcd")))
    End Sub

    <test>
    Private Shared Sub allow_unique_values()
        assertion.is_true(map.emplace_of(pair.emplace_of("abc", 1),
                                         pair.emplace_of("bcd", 2)).reverse(Nothing))
    End Sub

    <test>
    Private Shared Sub disallow_dup_values()
        assertion.is_false(map.emplace_of(pair.emplace_of("abc", 1),
                                          pair.emplace_of("bcd", 1)).reverse(Nothing))
    End Sub

    Private Sub New()
    End Sub
End Class
