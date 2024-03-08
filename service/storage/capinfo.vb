
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class capinfo
    Private ReadOnly di As DriveInfo
    Private ReadOnly reserved_disk_capacity As Int64

    Public Sub New(ByVal path_or_file As String, ByVal reserved_disk_capacity As UInt64)
        Try
            di = New DriveInfo(Path.GetPathRoot(Path.GetFullPath(path_or_file)))
            raise_error(error_type.information,
                        "drive info of ",
                        path_or_file,
                        " is ",
                        String.Join(", ",
                                    di.Name(),
                                    di.VolumeLabel(),
                                    di.DriveType(),
                                    di.DriveFormat(),
                                    di.TotalSize(),
                                    di.TotalFreeSpace(),
                                    di.AvailableFreeSpace()))
            If di.TotalFreeSpace() = 0 Then
                raise_error(error_type.warning, "drive of ",
                            path_or_file,
                            " returns zero free space. ",
                            "It either means the drive is readonly or has a flexible size (ramfs on linux). ",
                            "Writing into it is not guaranteed working, and capacity check will always return ",
                            "reserve_disk_capacity + 1 = ",
                            reserved_disk_capacity + 1)
            End If
        Catch ex As Exception
            raise_error(error_type.warning,
                        "fail to detect drive info of file or path ",
                        path_or_file,
                        ", ex ",
                        ex.Message(),
                        ", the capacity will always return 0")
        End Try
        assert(reserved_disk_capacity <= max_int64)
        Me.reserved_disk_capacity = CLng(reserved_disk_capacity)
    End Sub

    Public Sub New(ByVal path_or_file As String)
        Me.New(path_or_file, configuration.reserved_disk_capacity(path_or_file))
    End Sub

    Public Function capacity() As UInt64
        If di Is Nothing Then
            Return 0
        End If
        If di.TotalFreeSpace() = 0 Then
            Return CULng(reserved_disk_capacity + 1)
        End If
        Dim v As Int64 = do_(Function() di.AvailableFreeSpace(), int64_0) - reserved_disk_capacity
        If v <= 0 Then
            Return 0
        End If
        Return CULng(v)
    End Function

    Public Shared Operator +(ByVal this As capinfo) As UInt64
        If this Is Nothing Then
            Return 0
        End If
        Return this.capacity()
    End Operator
End Class
