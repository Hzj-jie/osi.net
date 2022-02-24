
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports breaking_word = osi.service.ml.wordtracer.cjk.breaking_word
Imports nd = osi.service.ml.normal_distribution
Imports ed = osi.service.ml.exponential_distribution

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class breaking_word_test
        <test>
        <command_line_specified>
        Private Shared Sub normal_distribution_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(
                           breaking_word.normal_distribute(model.load("cjk.words.2.raw.bin")), ms))
                assert(ms.dump_to_file("cjk.words.2.breaking_words.normal_distribute.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_normal_distribution_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.words.2.breaking_words.normal_distribute.bin"))
                ms.Position() = 0
                Dim r As unordered_map(Of Char, nd) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                Console.WriteLine(json_serializer.to_str(r))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub expo_distribution_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(breaking_word.expo_distribute(model.load("cjk.words.2.raw.bin")), ms))
                assert(ms.dump_to_file("cjk.words.2.breaking_words.expo_distribute.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_expo_distribution_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.words.2.breaking_words.expo_distribute.bin"))
                ms.Position() = 0
                Dim r As unordered_map(Of Char, ed) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                Console.WriteLine(json_serializer.to_str(
                    r.stream().
                      sort(Function(ByVal i As first_const_pair(Of Char, ed),
                                    ByVal j As first_const_pair(Of Char, ed)) As Int32
                               assert(Not i Is Nothing)
                               assert(Not j Is Nothing)
                               Return i.second.lambda.CompareTo(j.second.lambda)
                           End Function).
                      collect_to(Of vector(Of first_const_pair(Of Char, ed)))()))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub expo_possibilities_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(
                           breaking_word.expo_possibilities(model.load("cjk.words.2.raw.bin")), ms))
                assert(ms.dump_to_file("cjk.words.2.breaking_words.expo_possibilities.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_expo_possibilities_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.words.2.breaking_words.expo_possibilities.bin"))
                ms.Position() = 0
                Dim r As unordered_map(Of Char, unordered_map(Of Char, Double)) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                r.stream().
                  foreach(r.on_pair(Sub(ByVal k As Char, ByVal v As unordered_map(Of Char, Double))
                                        v.stream().
                                          sort(Function(ByVal i As first_const_pair(Of Char, Double),
                                                        ByVal j As first_const_pair(Of Char, Double)) As Int32
                                                   If i.second = j.second Then
                                                       Return i.first.CompareTo(j.first)
                                                   End If
                                                   Return comparer.reverse(i.second.CompareTo(j.second))
                                               End Function).
                                          foreach(v.on_pair(Sub(ByVal k2 As Char, ByVal p As Double)
                                                                Console.WriteLine(strcat(k, k2, ": ", p))
                                                            End Sub))
                                    End Sub))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub cumulative_distributes_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(
                           breaking_word.cumulative_distributes(model.load("cjk.words.2.raw.bin")), ms))
                assert(ms.dump_to_file("cjk.words.2.breaking_words.cumulative_distributes.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_cumulative_distributes_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.words.2.breaking_words.cumulative_distributes.bin"))
                ms.Position() = 0
                Dim r As unordered_map(Of Char, unordered_map(Of Char, Double)) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                r.stream().
                  foreach(r.on_pair(Sub(ByVal k As Char, ByVal v As unordered_map(Of Char, Double))
                                        v.stream().
                                          sort(Function(ByVal i As first_const_pair(Of Char, Double),
                                                        ByVal j As first_const_pair(Of Char, Double)) As Int32
                                                   If i.second = j.second Then
                                                       Return i.first.CompareTo(j.first)
                                                   End If
                                                   Return comparer.reverse(i.second.CompareTo(j.second))
                                               End Function).
                                          foreach(v.on_pair(Sub(ByVal k2 As Char, ByVal p As Double)
                                                                Console.WriteLine(strcat(k, k2, ": ", p))
                                                            End Sub))
                                    End Sub))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub cumulative_distributes_from_tracerall()
            Dim e As vector(Of unordered_map(Of String, Double)) = Nothing
            Using i As MemoryStream = New MemoryStream()
                assert(i.read_from_file("cjk.tracerall.bin"))
                i.Position() = 0
                Dim m As vector(Of unordered_map(Of String, UInt32)) = Nothing
                assert(bytes_serializer.consume_from(i, m))
                e = breaking_word.cumulative_distributes(m)
            End Using
            Using o As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(e, o))
                assert(o.dump_to_file("cjk.tracerall.expo_distribute.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_cumulative_distributes_from_tracerall()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.tracerall.expo_distribute.bin"))
                ms.Position() = 0
                ' Sort
                Dim r As vector(Of map(Of String, Double)) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                r.stream().
                  foreach(Sub(ByVal x As map(Of String, Double))
                              x.stream().
                                foreach(x.on_pair(Sub(ByVal k As String, ByVal v As Double)
                                                      Console.WriteLine(strcat(k, ": ", v))
                                                  End Sub))
                          End Sub)
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub bi_directional_expo_cumulative_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(
                       breaking_word.bi_directional_expo_cumulative(model.load("cjk.words.2.raw.bin")), ms))
                assert(ms.dump_to_file("cjk.words.2.breaking_words.bi_directional_expo_cumulative.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_bi_directional_expo_cumulative_from_raw()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.words.2.breaking_words.bi_directional_expo_cumulative.bin"))
                ms.Position() = 0
                Dim r As unordered_map(Of Char, unordered_map(Of Char, Double)) = Nothing
                assert(bytes_serializer.consume_from(ms, r))
                r.stream().
                  foreach(r.on_pair(Sub(ByVal k As Char, ByVal v As unordered_map(Of Char, Double))
                                        v.stream().
                                          sort(Function(ByVal i As first_const_pair(Of Char, Double),
                                                        ByVal j As first_const_pair(Of Char, Double)) As Int32
                                                   If i.second = j.second Then
                                                       Return i.first.CompareTo(j.first)
                                                   End If
                                                   Return comparer.reverse(i.second.CompareTo(j.second))
                                               End Function).
                                          foreach(v.on_pair(Sub(ByVal k2 As Char, ByVal p As Double)
                                                                Console.WriteLine(strcat(k, k2, ": ", p))
                                                            End Sub))
                                    End Sub))
            End Using
        End Sub
    End Class
End Namespace
