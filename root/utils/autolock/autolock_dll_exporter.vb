
#If 0 Then
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

<global_init(global_init_level.foundamental)>
Public Module autolock_dll_exporter
    Private ReadOnly autolock_path As String
    Private ReadOnly msvcr90_manifest_path As String
    Private ReadOnly msvcr90_path As String
    Private ReadOnly msvcm90_path As String
    Private ReadOnly msvcp90_path As String

    Sub New()
        autolock_path = IO.Path.Combine(application_directory, "osi.root.lock.autolock.dll")
        msvcr90_manifest_path = IO.Path.Combine(application_directory, "Microsoft.VC90.CRT.manifest")
        msvcr90_path = IO.Path.Combine(application_directory, "msvcr90.dll")
        msvcm90_path = IO.Path.Combine(application_directory, "msvcm90.dll")
        msvcp90_path = IO.Path.Combine(application_directory, "msvcp90.dll")

        Try
            IO.File.WriteAllBytes(autolock_path, autolock_dll_binary)
            IO.File.WriteAllBytes(msvcr90_manifest_path, If(x32_cpu, msvcr90_x86_manifest, msvcr90_amd64_manifest))
            IO.File.WriteAllBytes(msvcr90_path, If(x32_cpu, msvcr90_x86_dll, msvcr90_amd64_dll))
            IO.File.WriteAllBytes(msvcm90_path, If(x32_cpu, msvcm90_x86_dll, msvcm90_amd64_dll))
            IO.File.WriteAllBytes(msvcp90_path, If(x32_cpu, msvcp90_x86_dll, msvcp90_amd64_dll))
        Catch ex As Exception
            log_unhandled_exception(ex)
            assert(False)
        End Try

        application_lifetime.stopping_handle(Sub()
                                                 IO.File.Delete(autolock_path)
                                                 IO.File.Delete(msvcr90_manifest_path)
                                                 IO.File.Delete(msvcr90_path)
                                                 IO.File.Delete(msvcm90_path)
                                                 IO.File.Delete(msvcp90_path)
                                             End Sub)
    End Sub

    Private Sub init()
    End Sub
End Module
#End If
