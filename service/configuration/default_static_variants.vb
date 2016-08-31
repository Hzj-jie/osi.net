
Imports osi.root
Imports osi.root.formation
Imports osi.root.connector
Imports cons = osi.service.configuration.constants.static_filter

Friend Module _default_static_variants
    Private ReadOnly default_static_variants As vector(Of pair(Of String, String))

    Private Sub push(Of T)(ByVal n As String, ByVal v As T)
        default_static_variants.push_back(make_pair(n, Convert.ToString(v)))
    End Sub

    Public Function combine_static_variants(ByVal variants As vector(Of pair(Of String, String))) _
                                           As vector(Of pair(Of String, String))
        Dim v As vector(Of pair(Of String, String)) = Nothing
        copy(v, default_static_variants)
        If Not variants Is Nothing Then
            For i As Int32 = 0 To variants.size() - 1
                Dim f As Boolean = False
                For j As Int32 = 0 To v.size() - 1
                    If strsame(v(j).first, variants(i).first) Then
                        copy(v(j).second, variants(i).second)
                        f = True
                        Exit For
                    End If
                Next

                If Not f Then
                    v.push_back(variants(i))
                End If
            Next
        End If
        Return v
    End Function

    Sub New()
        default_static_variants = New vector(Of pair(Of String, String))()
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
        push(cons.service_name, envs.service_name)
        push(cons.short_version, envs.application_short_version)
        push(cons.total_physical_memory, envs.total_physical_memory())
        push(cons.total_virtual_memory, envs.total_virtual_memory())
        push(cons.user_name, envs.user_name)
        push(cons.version, envs.application_version)
        push(cons.working_directory, Environment.CurrentDirectory())
        push(cons.application_directory, envs.application_directory)
    End Sub
End Module
