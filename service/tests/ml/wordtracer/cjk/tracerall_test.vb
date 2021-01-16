
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports tracerall = osi.service.ml.wordtracer.cjk.tracerall

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class tracerall_test
        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            Dim m As vector(Of unordered_map(Of String, UInt32)) = New tracerall(0.1, 4).train("cjk.training.txt")
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(m, ms))
                assert(ms.dump_to_file("cjk.tracerall.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            Dim m As vector(Of unordered_map(Of String, UInt32)) = New tracerall(0.95, 4).train(
                tar.reader.unzip(vector.emplace_of(Directory.GetFiles(Environment.CurrentDirectory(),
                                                                      "tar_manual_test.zip_*",
                                                                      SearchOption.AllDirectories))))
            Using ms As MemoryStream = New MemoryStream()
                assert(bytes_serializer.append_to(m, ms))
                assert(ms.dump_to_file("cjk.tracerall.bin"))
            End Using
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            Using ms As MemoryStream = New MemoryStream()
                assert(ms.read_from_file("cjk.tracerall.bin"))
                ms.Position() = 0
                ' Ordered
                Dim m As vector(Of map(Of String, UInt32)) = Nothing
                assert(bytes_serializer.consume_from(ms, m))
                m.stream().
                  foreach(Sub(ByVal v As map(Of String, UInt32))
                              v.stream().
                                foreach(v.on_pair(Sub(ByVal k As String, ByVal c As UInt32)
                                                      Console.WriteLine(strcat(k, ": ", c))
                                                  End Sub))
                          End Sub)
            End Using
        End Sub
    End Class
End Namespace
