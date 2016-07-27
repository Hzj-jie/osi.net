
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Class capinfo
    Private ReadOnly di As DriveInfo
    Private ReadOnly preserve_disk_capacity As Int64

    Shared Sub New()
        assert(npos < 0)
    End Sub

    Public Sub New(ByVal path_or_file As String, ByVal preserve_disk_capacity As UInt64)
        Try
            di = New DriveInfo(Path.GetPathRoot(Path.GetFullPath(path_or_file)))
        Catch ex As Exception
            raise_error(error_type.warning,
                        "fail to detect drive info of file or path ",
                        path_or_file,
                        ", ex ",
                        ex.Message(),
                        ", the capacity will always return ",
                        npos)
        End Try
        assert(preserve_disk_capacity <= max_int64)
        Me.preserve_disk_capacity = preserve_disk_capacity
    End Sub

    Public Sub New(ByVal path_or_file As String)
        Me.New(path_or_file, configuration.preserved_disk_capacity(path_or_file))
    End Sub

    Public Function capacity() As Int64
        If di Is Nothing Then
            Return npos
        Else
            Return do_(Function() As Int64
                           Dim v As Int64 = 0
                           v = di.AvailableFreeSpace() - preserve_disk_capacity
                           If v <= 0 Then
                               Return 0
                           Else
                               Return v
                           End If
                       End Function,
                       npos)
        End If
    End Function

    Public Shared Operator +(ByVal this As capinfo) As Int64
        If this Is Nothing Then
            Return 0
        Else
            Return this.capacity()
        End If
    End Operator
End Class
