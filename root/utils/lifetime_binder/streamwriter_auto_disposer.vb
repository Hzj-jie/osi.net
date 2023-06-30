
Imports System.IO
Imports osi.root.formation

Public Class streamwriter_auto_disposer
    Inherits disposer(Of StreamWriter)

    Public Sub New(ByVal s As StreamWriter)
        MyBase.New(s)
    End Sub

    Private Shared Function create_stream_writer(ByVal file As String) As StreamWriter
        file = Path.GetFullPath(file)
        Directory.CreateDirectory(Path.GetDirectoryName(file))
        Dim s As StreamWriter = Nothing
        s = New StreamWriter(file)
        s.AutoFlush() = True
        Return s
    End Function

    Public Sub New(ByVal file As String)
        MyBase.New(create_stream_writer(file))
    End Sub
End Class
