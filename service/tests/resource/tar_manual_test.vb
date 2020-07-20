
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt.attributes
Imports osi.service.resource

<test>
Public NotInheritable Class tar_manual_test
    Private Shared Function list_files() As vector(Of String)
        Return vector.emplace_of(Directory.GetFiles(Environment.CurrentDirectory(), "*", SearchOption.AllDirectories)).
                      stream().
                      map(Function(ByVal s As String) As String
                              Return pather.default.relative_path(Environment.CurrentDirectory(), s)
                          End Function).
                      collect(Of vector(Of String))()
    End Function

    <command_line_specified>
    <test>
    Private Shared Sub pack_50m()
        Dim w As tar.writer = Nothing
        w = New tar.writer(50 * 1024 * 1024, "tar_manual_test.pack_50m.", list_files())
        assert(w.dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub zip_200m()
        Dim w As tar.writer = Nothing
        w = tar.writer.zip(200 * 1024 * 1024, "tar_manual_test.zip_200m.", list_files())
        assert(w.dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_console()
        Dim r As tar.reader = Nothing
        r = New tar.reader(vector.emplace_of(
                               Directory.GetFiles(Environment.CurrentDirectory(), "tar_manual_test.pack_*")))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Console.WriteLine(bytes_str(m.ToArray()))
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack()
        Dim r As tar.reader = Nothing
        r = New tar.reader(vector.emplace_of(
                               Directory.GetFiles(Environment.CurrentDirectory(), "tar_manual_test.pack_*")))
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_console()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(
                                 Directory.GetFiles(Environment.CurrentDirectory(), "tar_manual_test.zip_*")))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Console.WriteLine(bytes_str(m.ToArray()))
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(
                                 Directory.GetFiles(Environment.CurrentDirectory(), "tar_manual_test.zip_*")))
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    Private Sub New()
    End Sub
End Class
