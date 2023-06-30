
Imports System.IO
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock

Public Class localfile_datawatcher
    Inherits trigger_datawatcher

    Private ReadOnly watcher As FileSystemWatcher

    Private Sub New(ByVal filename As String)
        MyBase.New()
        assert(Not filename.null_or_empty())
        assert(Path.IsPathRooted(filename))
        Try
            If File.Exists(filename) Then
                trigger()
            End If
            watcher = New FileSystemWatcher(Path.GetDirectoryName(filename), Path.GetFileName(filename))
            watcher.NotifyFilter() = NotifyFilters.Attributes Or
                                     NotifyFilters.CreationTime Or
                                     NotifyFilters.FileName Or
                                     NotifyFilters.LastWrite Or
                                     NotifyFilters.Size
            AddHandler watcher.Changed, AddressOf changed
            AddHandler watcher.Created, AddressOf changed
            AddHandler watcher.Deleted, AddressOf changed
            AddHandler watcher.Renamed, AddressOf renamed
            AddHandler watcher.Error, AddressOf [error]
            watcher.EnableRaisingEvents() = True
        Catch ex As Exception
            assert(False, ex.Message())
        End Try
    End Sub

    Public Shared Function generate(ByVal file As String) As localfile_datawatcher
        file = assert_fullpath(file)
        Return collection.generate(strcat("localfile-datawatcher://", file),
                                   Function() New localfile_datawatcher(file))
    End Function

    Private Sub changed(ByVal source As Object, ByVal e As FileSystemEventArgs)
        trigger()
    End Sub

    Private Sub renamed(ByVal source As Object, ByVal e As RenamedEventArgs)
        trigger()
    End Sub

    Private Sub [error](ByVal source As Object, ByVal e As ErrorEventArgs)
        watcher.EnableRaisingEvents() = False
        watcher.EnableRaisingEvents() = True
    End Sub
End Class
