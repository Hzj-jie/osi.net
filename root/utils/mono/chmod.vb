
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Public Module _chmod
    Public Enum FilePermissions As UInt32
        S_ISUID = &H800 ' Set user ID on execution
        S_ISGID = &H400 ' Set group ID on execution
        S_ISVTX = &H200 ' Save swapped text after use (sticky).
        S_IRUSR = &H100 ' Read by owner
        S_IWUSR = &H80 ' Write by owner
        S_IXUSR = &H40 ' Execute by owner
        S_IRGRP = &H20 ' Read by group
        S_IWGRP = &H10 ' Write by group
        S_IXGRP = &H8 ' Execute by group
        S_IROTH = &H4 ' Read by other
        S_IWOTH = &H2 ' Write by other
        S_IXOTH = &H1 ' Execute by other

        S_IRWXG = (S_IRGRP Or S_IWGRP Or S_IXGRP)
        S_IRWXU = (S_IRUSR Or S_IWUSR Or S_IXUSR)
        S_IRWXO = (S_IROTH Or S_IWOTH Or S_IXOTH)
        ACCESSPERMS = (S_IRWXU Or S_IRWXG Or S_IRWXO) ' 0777
        ALLPERMS = (S_ISUID Or S_ISGID Or S_ISVTX Or S_IRWXU Or S_IRWXG Or S_IRWXO) ' 07777
        DEFFILEMODE = (S_IRUSR Or S_IWUSR Or S_IRGRP Or S_IWGRP Or S_IROTH Or S_IWOTH) ' 0666

        ' Device types
        ' Why these are held in "mode_t" is beyond me...
        S_IFMT = &HF000 ' Bits which determine file type
        S_IFDIR = &H4000 ' Directory
        S_IFCHR = &H2000 ' Character device
        S_IFBLK = &H6000 ' Block device
        S_IFREG = &H8000 ' Regular file
        S_IFIFO = &H1000 ' FIFO
        S_IFLNK = &HA000 ' Symbolic link
        S_IFSOCK = &HC000 ' Socket
    End Enum

    Private Const mono_posix_assembly_v2 As String =
                      "Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756"
    Private Const mono_posix_assembly_v4 As String =
                      "Mono.Posix, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756"
    Private ReadOnly f As invoker(Of not_resolved_type_delegate)

    Sub New()
        If envs.mono Then
            Try
                If envs.clr_2 Then
                    AppDomain.CurrentDomain().Load(mono_posix_assembly_v2)
                ElseIf envs.clr_4 Then
                    AppDomain.CurrentDomain().Load(mono_posix_assembly_v4)
                Else
                    raise_error(error_type.warning,
                                "Mono.Posix assembly is not supporting CLR other than 2.0 or 4.0")
                    Return
                End If
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to load Mono.Posix assembly, ex ",
                            ex.Message())
                Return
            End Try
            ' Mono.Unix.Native.FilePermissions is not available.
            typeless_invoker.of(f).
                with_type_name(strcat("Mono.Unix.Native.Syscall, ",
                                      If(envs.clr_2, mono_posix_assembly_v2, mono_posix_assembly_v4))).
                with_name("chmod").
                build(f)
        End If
    End Sub

    Public Sub chmod(ByVal file As String, ByVal permissions As FilePermissions, ByRef o As Int32)
        If Not f Is Nothing AndAlso f.static() AndAlso f.pre_binding() Then
            o = direct_cast(Of Int32)(f.invoke(Nothing, file, CUInt(permissions)))
        Else
            o = -2
        End If
    End Sub

    Public Function chmod(ByVal file As String, ByVal permissions As FilePermissions) As Boolean
        Dim o As Int32 = 0
        chmod(file, permissions, o)
        Return (o = 0)
    End Function

    Public Function chmod_exe(ByVal file As String) As Boolean
        Return chmod(file, FilePermissions.S_IXUSR Or FilePermissions.S_IRUSR)
    End Function
End Module
