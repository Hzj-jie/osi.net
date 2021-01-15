
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt.attributes
Imports osi.service.resource

<test>
Public NotInheritable Class tar_manual_test
    Private Const pack_50m_file As String = "tar_manual_test.pack_50m."
    Private Const pack_files As String = "tar_manual_test.pack_*"
    Private Const zip_200m_file As String = "tar_manual_test.zip_200m."
    Private Const zip_files As String = "tar_manual_test.zip_*"
    Private Const peek_size As UInt32 = 512
    Private Const peek_lines As UInt32 = 3

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
        assert(New tar.writer(50 * 1024 * 1024, pack_50m_file, list_files()).dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub pack_50m_append()
        assert(New tar.writer(50 * 1024 * 1024, pack_50m_file, list_files()).append())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub zip_200m()
        assert(tar.writer.zip(200 * 1024 * 1024, zip_200m_file, list_files()).dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub zip_200m_append()
        assert(tar.writer.zip(200 * 1024 * 1024, zip_200m_file, list_files()).append())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_console()
        Dim r As tar.reader = Nothing
        r = New tar.reader(vector.emplace_of(
                           Directory.GetFiles(Environment.CurrentDirectory(), pack_files, SearchOption.AllDirectories)))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, True)
                          Console.WriteLine(sr.ReadToEnd())
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_index()
        Dim r As tar.reader = Nothing
        r = New tar.reader(vector.emplace_of(Directory.GetFiles(
                                             Environment.CurrentDirectory(), pack_files, SearchOption.AllDirectories)))
        r.index().stream().foreach(Sub(ByVal s As tuple(Of String, UInt32))
                                       Console.WriteLine(strcat(s.first(), " -- ", s.second()))
                                   End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_peek()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(Directory.GetFiles(
                                               Environment.CurrentDirectory(), zip_files, SearchOption.AllDirectories)))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      m.trim(peek_size * peek_lines)
                      Using sr As StreamReader = New StreamReader(m, True)
                          For i As UInt32 = 0 To peek_lines - uint32_1
                              Console.WriteLine(sr.ReadLine())
                          Next
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack()
        Dim r As tar.reader = Nothing
        r = New tar.reader(vector.emplace_of(Directory.GetFiles(
                                             Environment.CurrentDirectory(), pack_files, SearchOption.AllDirectories)))
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_console()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(Directory.GetFiles(
                                               Environment.CurrentDirectory(), zip_files, SearchOption.AllDirectories)))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, True)
                          Console.WriteLine(sr.ReadToEnd())
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_index()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(Directory.GetFiles(
                                               Environment.CurrentDirectory(), zip_files, SearchOption.AllDirectories)))
        r.index().stream().foreach(Sub(ByVal s As tuple(Of String, UInt32))
                                       Console.WriteLine(strcat(s.first(), " -- ", s.second()))
                                   End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_peek()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(Directory.GetFiles(
                                               Environment.CurrentDirectory(), zip_files, SearchOption.AllDirectories)))
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      m.trim(peek_size * peek_lines)
                      Using sr As StreamReader = New StreamReader(m, True)
                          For i As UInt32 = 0 To peek_lines - uint32_1
                              Console.WriteLine(sr.ReadLine())
                          Next
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip()
        Dim r As tar.reader = Nothing
        r = tar.reader.unzip(vector.emplace_of(Directory.GetFiles(
                                               Environment.CurrentDirectory(), zip_files, SearchOption.AllDirectories)))
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    Private Sub New()
    End Sub
End Class
