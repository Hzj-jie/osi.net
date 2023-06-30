
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root
Imports osi.root.connector
Imports osi.root.formation
Imports cons = osi.service.configuration.constants.static_filter

Friend Module _default_static_variants
    Private ReadOnly default_static_variants As vector(Of pair(Of String, String)) =
        Function() As vector(Of pair(Of String, String))
            Dim r As New vector(Of pair(Of String, String))()
            Dim push As Action(Of String, Object) = Sub(ByVal s As String, ByVal v As Object)
                                                        r.emplace_back(pair.emplace_of(s, Convert.ToString(v)))
                                                    End Sub
            push(cons.available_physical_memory, envs.available_physical_memory())
            push(cons.available_virtual_memory, envs.available_virtual_memory())
            push(cons.build, envs.build)
            push(cons.computer_name, envs.computer_name)
            push(cons.domain_name, envs.domain_name)
            push(cons.machine_name, envs.machine_name)
            push(cons.os_full_name, envs.os.full_name)
            push(cons.os_platform, envs.os.platform)
            push(cons.os_version, envs.os.version)
            push(cons.processor_count, Environment.ProcessorCount())
            push(cons.running_mode, envs.running_mode)
            push(cons.service_name, envs.deploys.service_name)
            push(cons.short_version, envs.application_short_version)
            push(cons.total_physical_memory, envs.total_physical_memory())
            push(cons.total_virtual_memory, envs.total_virtual_memory())
            push(cons.user_name, envs.user_name)
            push(cons.version, envs.application_version)
            push(cons.working_directory, Environment.CurrentDirectory())
            push(cons.application_directory, envs.deploys.service_name)
            Return r
        End Function()

    Public Function combine_static_variants(ByVal variants As vector(Of pair(Of String, String))) _
                                           As vector(Of pair(Of String, String))
        Dim v As vector(Of pair(Of String, String)) = default_static_variants.CloneT()
        For i As Int32 = 0 To CInt(variants.size_or_0()) - 1
            Dim f As Boolean = False
            For j As Int32 = 0 To CInt(v.size()) - 1
                If String.Equals(v(CUInt(j)).first, variants(CUInt(i)).first) Then
                    copy(v(CUInt(j)).second, variants(CUInt(i)).second)
                    f = True
                    Exit For
                End If
            Next

            If Not f Then
                v.push_back(variants(CUInt(i)))
            End If
        Next
        Return v
    End Function
End Module
