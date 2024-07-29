
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector

Public NotInheritable Class reserved_disk_capacity
    Private Const default_reserved_capacity As UInt64 = 64 * 1024 * 1024
    Private ReadOnly m As New map(Of String, UInt64)()
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

    Default Public Property reserved_disk_capacity(ByVal path As String) As UInt64
        Get
            If Not pathroot(path, path) Then
                Return default_reserved_capacity
            End If
            Return l.reader_locked(Function() As UInt64
                                       Dim it As map(Of String, UInt64).iterator = m.find(path)
                                       If it = m.end() Then
                                           Return default_reserved_capacity
                                       End If
                                       Return (+it).second
                                   End Function)
        End Get
        Set(value As UInt64)
            If pathroot(path, path) Then
                l.writer_locked(Sub()
                                    m(path) = value
                                End Sub)
            End If
        End Set
    End Property
End Class

