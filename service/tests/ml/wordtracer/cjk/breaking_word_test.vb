
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
                      collect(Of vector(Of first_const_pair(Of Char, ed)))()))
            End Using
        End Sub
    End Class
End Namespace
