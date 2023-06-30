
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.resource

<test>
Public NotInheritable Class tar_test
    <test>
    Private Shared Sub dump()
        Dim v As vector(Of String) = vector.of("a".str_repeat(5),
                                               "b".str_repeat(5),
                                               "c".str_repeat(2),
                                               "d".str_repeat(2),
                                               "e".str_repeat(3),
                                               "f".str_repeat(9),
                                               "g".str_repeat(2),
                                               "h".str_repeat(3),
                                               "i".str_repeat(10),
                                               "y".str_repeat(100),
                                               "z".str_repeat(1))
        Dim fs As tar.testing_fs = New tar.testing_fs().with(v)
        assertion.is_true(tar.writer.of_testing(fs, 36).dump())
        fs.erase(v)
        assertion.equal(fs.list_files().size(), CUInt(6))
        Dim v2 As vector(Of tuple(Of String, MemoryStream)) = tar.reader.of_testing(fs).dump()
        assertion.equal(v,
                        v2.stream().
                           map(Function(ByVal t As tuple(Of String, MemoryStream)) As String
                                   assertion.equal(fs.stream_of(t.first()).unread_compare_to(t.second()), 0)
                                   Return t.first()
                               End Function).
                           collect_to(Of vector(Of String))())
    End Sub

    <test>
    Private Shared Sub dump_single_file()
        Dim v As vector(Of String) = vector.of("abc")
        Dim fs As tar.testing_fs = New tar.testing_fs().with(v)
        assertion.is_true(tar.writer.of_testing(fs, 100).dump())
        fs.erase(v)
        assertion.equal(fs.list_files().size(), uint32_1)
        Dim v2 As vector(Of tuple(Of String, MemoryStream)) = tar.reader.of_testing(fs).dump()
        assertion.equal(v,
                        v2.stream().
                           map(Function(ByVal t As tuple(Of String, MemoryStream)) As String
                                   assertion.equal(fs.stream_of(t.first()).unread_compare_to(t.second()), 0)
                                   Return t.first()
                               End Function).
                           collect_to(Of vector(Of String))())
    End Sub

    <test>
    Private Shared Sub append()
        Dim v1 As vector(Of String) = vector.of("a".str_repeat(5),
                                                "b".str_repeat(5))
        Dim v2 As vector(Of String) = vector.of("c".str_repeat(2),
                                                "d".str_repeat(2),
                                                "e".str_repeat(3),
                                                "f".str_repeat(9),
                                                "g".str_repeat(2))
        Dim v3 As vector(Of String) = vector.of("h".str_repeat(3),
                                                "i".str_repeat(10))
        Dim v4 As vector(Of String) = vector.of("j".str_repeat(3),
                                                "k".str_repeat(10),
                                                "l".str_repeat(10),
                                                "m".str_repeat(10),
                                                "n".str_repeat(10))
        Dim v5 As vector(Of String) = vector.of("o".str_repeat(3),
                                                "p".str_repeat(10))
        Dim v6 As vector(Of String) = vector.of("y".str_repeat(100))
        Dim v7 As vector(Of String) = vector.of("z".str_repeat(1))
        Dim fs As tar.testing_fs = New tar.testing_fs().with(v1, v2, v3, v4, v5, v6, v7)
        assertion.is_true(tar.writer.of_testing(fs, 36, v1).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v2).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v3).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v4).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v5).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v6).append())
        assertion.is_true(tar.writer.of_testing(fs, 36, v7).append())
        fs.erase(v1, v2, v3, v4, v5, v6, v7)
        assertion.equal(fs.list_files().size(), CUInt(10))
        Dim v As vector(Of tuple(Of String, MemoryStream)) = tar.reader.of_testing(fs).dump()
        assertion.equal(v.stream().
                          map(Function(ByVal t As tuple(Of String, MemoryStream)) As String
                                  assertion.equal(fs.stream_of(t.first()).unread_compare_to(t.second()), 0)
                                  Return t.first()
                              End Function).
                          collect_to(Of vector(Of String))(),
                        streams.of(v1, v2, v3, v4, v5, v6, v7).
                                flat_map(Function(ByVal x As vector(Of String)) As stream(Of String)
                                             Return x.stream()
                                         End Function).
                                collect_to(Of vector(Of String))())
    End Sub

    Private Sub New()
    End Sub
End Class
