
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
    Private Shared Sub run()
        Dim v As vector(Of String) = Nothing
        v = vector.of("a".str_repeat(5),
                      "b".str_repeat(5),
                      "c".str_repeat(2),
                      "d".str_repeat(2),
                      "e".str_repeat(3),
                      "f".str_repeat(9),
                      "g".str_repeat(2),
                      "h".str_repeat(3),
                      "i".str_repeat(10),
                      "z".str_repeat(100))
        Dim w As tar.writer = Nothing
        Dim output As vector(Of MemoryStream) = Nothing
        output = New vector(Of MemoryStream)()
        w = tar.writer.of_testing(36, v, output)
        assertion.is_true(w.dump())
        assertion.equal(output.size(), CUInt(5))
        Dim r As tar.reader = Nothing
        r = tar.reader.of_testing(output)
        Dim v2 As vector(Of tuple(Of String, MemoryStream)) = Nothing
        v2 = r.dump()
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
