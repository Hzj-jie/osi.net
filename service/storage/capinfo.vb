
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

Public Class capinfo
    Private ReadOnly di As DriveInfo
    Private ReadOnly reserve_disk_capacity As Int64

    Shared Sub New()
        assert(npos < 0)
    End Sub

    Public Sub New(ByVal path_or_file As String, ByVal reserve_disk_capacity As UInt64)
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
        assert(reserve_disk_capacity <= max_int64)
        Me.reserve_disk_capacity = CLng(reserve_disk_capacity)
    End Sub

    Public Sub New(ByVal path_or_file As String)
        Me.New(path_or_file, configuration.preserved_disk_capacity(path_or_file))
    End Sub

    Public Function capacity() As UInt64
        If di Is Nothing Then
            Return 0
        Else
            Dim v As Int64 = 0
            v = do_(Function() di.AvailableFreeSpace(), int64_0) - reserve_disk_capacity
            If v <= 0 Then
                Return 0
            Else
                Return CULng(v)
            End If
        End If
    End Function

    Public Shared Operator +(ByVal this As capinfo) As UInt64
        If this Is Nothing Then
            Return 0
        Else
            Return this.capacity()
        End If
    End Operator
End Class
