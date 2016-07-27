
Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Public Class streamwriter_auto_disposer
    Inherits disposer(Of StreamWriter)

    Public Sub New(ByVal s As StreamWriter)
        MyBase.New(s)
    End Sub

    Public Sub New(ByVal file As String)
        MyBase.New(Function() As StreamWriter
                       Dim s As StreamWriter = Nothing
                       s = New StreamWriter(file)
                       s.AutoFlush() = True
                       Return s
                   End Function,
                   Sub()
                       file = Path.GetFullPath(file)
                       Directory.CreateDirectory(Path.GetDirectoryName(file))
                   End Sub,
                   Sub(i As StreamWriter)
                       close_writer(i)
                   End Sub)
    End Sub
End Class
