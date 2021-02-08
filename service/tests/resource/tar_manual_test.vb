
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.resource

<test>
Public NotInheritable Class tar_manual_test
    Private Const pack_50m_file As String = "tar_manual_test.pack_50m."
    Private Const pack_files As String = "tar_manual_test.pack_*"
    Private Const zip_200m_file As String = "tar_manual_test.zip_200m."
    Private Const zip_files As String = "tar_manual_test.zip_*"
    Private Const peek_size As UInt32 = 128

    <command_line_specified>
    <test>
    Private Shared Sub pack_50m()
        assert(New tar.writer(50 * 1024 * 1024, pack_50m_file, New tar.selector()).dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub pack_50m_append()
        assert(New tar.writer(50 * 1024 * 1024, pack_50m_file, New tar.selector()).append())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub zip_200m()
        assert(tar.writer.zip(200 * 1024 * 1024, zip_200m_file, New tar.selector()).dump())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub zip_200m_append()
        assert(tar.writer.zip(200 * 1024 * 1024, zip_200m_file, New tar.selector()).append())
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_console()
        Dim r As New tar.reader(New tar.selector() With {.pattern = pack_files})
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, m.guess_encoding())
                          Console.WriteLine(sr.ReadToEnd())
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_index()
        Dim r As New tar.reader(New tar.selector() With {.pattern = pack_files})
        r.index().stream().foreach(Sub(ByVal s As tuple(Of String, UInt32))
                                       Console.WriteLine(strcat(s.first(), " -- ", s.second()))
                                   End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack_peek()
        Dim r As New tar.reader(New tar.selector() With {.pattern = zip_files})
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, m.guess_encoding())
                          Dim c(peek_size - 1) As Char
                          Dim l As Int32 = sr.Read(c, 0, c.array_size_i())
                          Console.WriteLine(New String(c, 0, l))
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unpack()
        Dim r As New tar.reader(New tar.selector() With {.pattern = pack_files})
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_console()
        Dim r As tar.reader = tar.reader.unzip(New tar.selector() With {.pattern = zip_files})
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, m.guess_encoding())
                          Console.WriteLine(sr.ReadToEnd())
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_index()
        Dim r As tar.reader = tar.reader.unzip(New tar.selector() With {.pattern = zip_files})
        r.index().stream().foreach(Sub(ByVal s As tuple(Of String, UInt32))
                                       Console.WriteLine(strcat(s.first(), " -- ", s.second()))
                                   End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip_peek()
        Dim r As tar.reader = tar.reader.unzip(New tar.selector() With {.pattern = zip_files})
        r.foreach(Sub(ByVal s As String, ByVal m As MemoryStream)
                      Console.WriteLine(strcat(s, " ========"))
                      Using sr As StreamReader = New StreamReader(m, m.guess_encoding())
                          Dim c(peek_size - 1) As Char
                          Dim l As Int32 = sr.Read(c, 0, c.array_size_i())
                          Console.WriteLine(New String(c, 0, l))
                      End Using
                  End Sub)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub unzip()
        Dim r As tar.reader = tar.reader.unzip(New tar.selector() With {.pattern = zip_files})
        r.dump().stream().foreach(Sub(ByVal t As tuple(Of String, MemoryStream))
                                      File.WriteAllBytes(t.first(), t.second().ToArray())
                                  End Sub)
    End Sub

    Private Sub New()
    End Sub
End Class
