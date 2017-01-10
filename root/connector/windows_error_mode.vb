
Imports osi.root.constants

Public NotInheritable Class windows_error_mode
    Private Declare Function SetErrorMode Lib "kernel32.dll" (ByVal mode As UInt32) As UInt32
    Private Declare Function GetErrorMode Lib "kernel32.dll" () As UInt32

    Public Const SYSTEM_DEFAULT As UInt32 = &H0
    Public Const SEM_FAILCRITICALERRORS As UInt32 = &H1
    Public Const SEM_NOGPFAULTERRORBOX As UInt32 = &H2
    Public Const SEM_NOALIGNMENTFAULTEXCEPT As UInt32 = &H4
    Public Const SEM_NOOPENFILEERRORBOX As UInt32 = &H8000

    Public Shared Function [set](ByVal mode As UInt32, Optional ByRef old_mode As UInt32 = uint32_0) As Boolean
        Try
            old_mode = SetErrorMode(mode)
            Return True
        Catch ex As DllNotFoundException
            Return False
        End Try
    End Function

    Public Shared Function [get](ByRef mode As UInt32) As Boolean
        Try
            mode = GetErrorMode()
            Return True
        Catch ex As DllNotFoundException
            Return False
        End Try
    End Function

    Private Sub New()
    End Sub
End Class

<global_init(global_init_level.foundamental)>
Public NotInheritable Class disable_windows_error_popup
    Shared Sub New()
        windows_error_mode.[set](windows_error_mode.SEM_FAILCRITICALERRORS Or
                                 windows_error_mode.SEM_NOALIGNMENTFAULTEXCEPT Or
                                 windows_error_mode.SEM_NOGPFAULTERRORBOX Or
                                 windows_error_mode.SEM_NOOPENFILEERRORBOX)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
