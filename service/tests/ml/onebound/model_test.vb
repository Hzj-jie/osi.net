
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of String)
Imports osi.service.resources

Namespace onebound
    <test>
    Public NotInheritable Class model_test
        Private Shared input As argument(Of String)
        Private Shared inputs As argument(Of vector(Of String))
        Private Shared lower_bound As argument(Of Double)
        Private Shared output As argument(Of String)
        Private Shared ratio As argument(Of Double)

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
                  filter(lower_bound Or 0).
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

        Private Shared Function pure_words() As unordered_map(Of String, Double)
            Dim m As unordered_map(Of String, Double) =
                model.load(+input).
                      filter(lower_bound Or 0).
                      flat_map().
                      map(Function(ByVal p As first_const_pair(Of const_pair(Of String, String), Double)) _
                              As first_const_pair(Of String, Double)
                              Return first_const_pair.emplace_of(strcat(p.first.first, p.first.second), p.second)
                          End Function).
                      collect(Of unordered_map(Of String, Double))()
            Dim it As unordered_map(Of String, Double).iterator = m.begin()
            While it <> m.end()
                Dim s As String = (+it).first
                If s.strlen() > 2 Then
                    Dim comparison As Action(Of unordered_map(Of String, Double).iterator) =
                        Sub(ByVal it2 As unordered_map(Of String, Double).iterator)
                            If (+it2).second > (+it).second * (ratio Or 1) Then
                                it.value().second = 0
                            ElseIf (+it).second > (+it2).second * (ratio Or 1) Then
                                it2.value().second = 0
                            End If
                        End Sub
                    comparison(m.find(s.strleft(s.strlen() - uint32_1)))
                    comparison(m.find(s.strmid(uint32_1)))
                End If
                it += 1
            End While

            it = m.begin()
            While it <> m.end()
                If (+it).second = 0 Then
                    it = m.erase(it)
                Else
                    it += 1
                End If
            End While

            Return m
        End Function

        <command_line_specified>
        <test>
        Private Shared Sub remove_sub_super_words()
            Dim m As unordered_map(Of String, Double) = pure_words()
            Dim it As unordered_map(Of String, Double).iterator = m.begin()
            While it <> m.end()
                Console.WriteLine(strcat((+it).first, " ", (+it).second))
                it += 1
            End While
        End Sub

        Private Shared Function golden_words() As unordered_set(Of String)
            Return (+zh.words()).stream().
                                 filter(Function(ByVal i As String) As Boolean
                                            Return i.strlen() > 1
                                        End Function).
                                 collect(Of unordered_set(Of String))()
        End Function

        <command_line_specified>
        <test>
        Private Shared Sub unmatched_words()
            Dim s As unordered_set(Of String) = golden_words()
            Dim m As unordered_map(Of String, Double) = pure_words()
            Dim it As unordered_map(Of String, Double).iterator = m.begin()
            While it <> m.end()
                If s.find((+it).first) = s.end() Then
                    Console.WriteLine((+it).first)
                End If
                it += 1
            End While
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub matched_words()
            Dim s As unordered_set(Of String) = golden_words()
            Dim m As unordered_map(Of String, Double) = pure_words()
            Dim it As unordered_map(Of String, Double).iterator = m.begin()
            While it <> m.end()
                If s.find((+it).first) <> s.end() Then
                    Console.WriteLine((+it).first)
                End If
                it += 1
            End While
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub missing_words()
            Dim s As unordered_set(Of String) = golden_words()
            Dim m As unordered_map(Of String, Double) = pure_words()
            Dim it As unordered_set(Of String).iterator = s.begin()
            While it <> s.end()
                If m.find(+it) = m.end() Then
                    Console.WriteLine(+it)
                End If
                it += 1
            End While
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub golden_words_to_console()
            golden_words().stream().
                           foreach(Sub(ByVal x As String)
                                       Console.WriteLine(x)
                                   End Sub)
        End Sub

        <command_line_specified>
        <test>
        Private Shared Sub unique_chars()
            pure_words().stream().
                         flat_map(Function(ByVal x As first_const_pair(Of String, Double)) As stream(Of Char)
                                      Return streams.of(x.first.ToCharArray())
                                  End Function).
                         collect_by(stream(Of Char).collectors.unique()).
                         stream().
                         foreach(Sub(ByVal c As Char)
                                     Console.WriteLine(c)
                                 End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
