
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public Class data_dir
    Private ReadOnly base_dir As String

    Public Sub New(ByVal data_dir As String)
        Try
            base_dir = Path.GetFullPath(data_dir)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "cannot get full path of ",
                        data_dir,
                        ", ex ",
                        ex.Message(),
                        ", the file_key will keep in invalid status.")
            Return
        End Try
        assert(Not base_dir.null_or_empty())
        If base_dir.EndsWith(Path.DirectorySeparatorChar) Then
            base_dir = strleft(base_dir, strlen(base_dir) - strlen(Path.DirectorySeparatorChar))
        End If
        assert(Not create_directory(True) OrElse valid())
    End Sub

    Private Function create_directory(ByVal msg As Boolean) As Boolean
        Try
            Directory.CreateDirectory(base_dir)
            Return True
        Catch ex As Exception
            If msg Then
                raise_error(error_type.warning,
                            "cannot create directory ",
                            base_dir,
                            ", ex ",
                            ex.Message(),
                            ", the file_key will keep in invalid status.")
            End If
            Return False
        End Try
    End Function

    Public Function clear() As Boolean
        Try
            Directory.Delete(base_dir, True)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "cannot delete directory ",
                        base_dir,
                        ", ex ",
                        ex.Message())
            Return False
        End Try
        Return create_directory(True)
    End Function

    Public Function base_directory() As String
        Return base_dir
    End Function

    Public Function create_directory() As Boolean
        Return create_directory(False)
    End Function

    Public Function valid() As Boolean
        Return do_(Function() As Boolean
                       Return Directory.Exists(base_dir)
                   End Function,
                   False)
    End Function

    Public Function fullpath(ByVal file_or_path As String) As String
        Return Path.Combine(base_dir, file_or_path)
    End Function

    Public Function relative_path(ByVal full_file_or_path As String) As String
        Return strmid(full_file_or_path, strlen(base_dir) + strlen(Path.DirectorySeparatorChar))
    End Function
End Class
