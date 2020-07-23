
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
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
        Dim w As tar.writer = tar.writer.of_testing(fs, 36)
        assertion.is_true(w.dump())
        fs.erase(v)
        assertion.equal(fs.list_files().size(), CUInt(6))
        Dim v2 As vector(Of tuple(Of String, MemoryStream)) = tar.reader.of_testing(fs).dump()
        assertion.equal(v,
                        v2.stream().
                           map(Function(ByVal t As tuple(Of String, MemoryStream)) As String
                                   assertion.equal(memory_stream.of(t.first()).compare_to(t.second()), 0)
                                   Return t.first()
                               End Function).
                           collect(Of vector(Of String))())
    End Sub

    Private Sub New()
    End Sub
End Class
