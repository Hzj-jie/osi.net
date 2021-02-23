
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of String)

Namespace onebound
    <test>
    Public NotInheritable Class model_test
        Private Shared input As argument(Of String)
        Private Shared inputs As argument(Of vector(Of String))
        Private Shared lower_bound As argument(Of Double)
        Private Shared output As argument(Of String)

        <test>
        <repeat(100)>
        Private Shared Sub load_and_dump()
            Using ms As MemoryStream = New MemoryStream()
                Dim t As trainer = Nothing
                t = New trainer()
                For i As Int32 = 0 To 100
                    Dim a As String = Nothing
                    a = guid_str()
                    Dim b As String = Nothing
                    b = guid_str()
                    t.accumulate(a, b, thread_random.of_double.larger_than_0_and_less_or_equal_than_1())
                Next
                Dim m As model = Nothing
                m = t.dump()
                assertion.is_true(m.dump(ms))
                ms.Position() = 0
                Dim m2 As model = Nothing
                assertion.is_true(model.load(ms, m2))
                assertion.equal(m, m2)
            End Using
        End Sub

        <test>
        Private Shared Sub reverse_test()
            Dim m As model = New trainer().
                                 accumulate("a", "b").
                                 accumulate("a", "c").
                                 accumulate("a", "c").
                                 accumulate("a", "d").
                                 accumulate("a", "d").
                                 accumulate("a", "d").
                                 dump().
                                 reverse()
            assertion.equal(m.affinity("d", "a"), 3)
            assertion.equal(m.affinity("c", "a"), 2)
            assertion.equal(m.affinity("b", "a"), 1)
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub combo_load()
            model.combo_load(++inputs).filter(lower_bound Or 0).dump(output Or "combo.bin")
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub sort_to_console()
            model.load(+input).
                  flat_map().
                  map(Function(ByVal p As first_const_pair(Of const_pair(Of String, String), Double)) _
                          As first_const_pair(Of String, Double)
                          Return first_const_pair.emplace_of(strcat(p.first.first, p.first.second), p.second)
                      End Function).
                  sort().
                  foreach(Sub(ByVal p As first_const_pair(Of String, Double))
                              Console.WriteLine(strcat(p.first, " ", p.second))
                          End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
