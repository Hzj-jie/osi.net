
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Public Class preserved_disk_capacity
    Private ReadOnly d As UInt64
    Private ReadOnly m As map(Of String, UInt64)
    Private l As duallock

    Private Shared Function pathroot(ByVal path As String, ByRef r As String) As Boolean
        Try
            r = IO.Path.GetPathRoot(IO.Path.GetFullPath(path))
            Return True
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to detect root of the path ",
                        path,
                        ", ex ",
                        ex.Message(),
                        ", will return default value.")
            Return False
        End Try
    End Function

    Default Public Property preserved_disk_capacity(ByVal path As String) As UInt64
        Get
            If pathroot(path, path) Then
                Return l.reader_locked(Function() As UInt64
                                           Dim it As map(Of String, UInt64).iterator = Nothing
                                           it = m.find(path)
                                           If it = m.end() Then
                                               Return d
                                           Else
                                               Return (+it).second
                                           End If
                                       End Function)
            Else
                Return d
            End If
        End Get
        Set(value As UInt64)
            If pathroot(path, path) Then
                l.writer_locked(Sub()
                                    m(path) = value
                                End Sub)
            End If
        End Set
    End Property

    Public Sub New()
        d = 256 * 1024 * 1024
        m = New map(Of String, UInt64)()
    End Sub
End Class
