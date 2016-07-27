
Imports System.Management
Imports osi.root.connector

Public Module _hardware
    Public ReadOnly hardware_manufacturer As String
    Public ReadOnly hardware_model As String
    Public ReadOnly hyper_v_virtual_machine As Boolean
    Public ReadOnly vmware_virtual_machine As Boolean
    Public ReadOnly virtualbox_virtual_machine As Boolean
    Public ReadOnly virtual_machine As Boolean
    Public ReadOnly report_hardware_info As Boolean

    Sub New()
        If envs.mono Then
            hardware_manufacturer = "Mono"
            hardware_model = "Mono"
        Else
            Using searcher = New ManagementObjectSearcher("select * from Win32_ComputerSystem")
                Using items = searcher.Get()
                    For Each item In items
                        Using item
                            Dim s As String = Nothing
                            s = item("Manufacturer")
                            If Not String.IsNullOrEmpty(s) Then
                                hardware_manufacturer = s
                            End If
                            s = item("Model")
                            If Not String.IsNullOrEmpty(s) Then
                                hardware_model = s
                            End If
                        End Using
                    Next
                End Using
            End Using
        End If
        hyper_v_virtual_machine = (strsame(hardware_manufacturer, "microsoft corporation", False) AndAlso
                                   strcontains(hardware_model, "virtual", False))
        vmware_virtual_machine = strcontains(hardware_manufacturer, "vmware", False)
        virtualbox_virtual_machine = strsame(hardware_model, "virtualbox", False)
        virtual_machine = (hyper_v_virtual_machine OrElse
                           vmware_virtual_machine OrElse
                           virtualbox_virtual_machine)
        report_hardware_info = env_bool(env_keys("report", "hardware", "info"))
        If report_hardware_info Then
            raise_error("manufacturer ", hardware_manufacturer)
            raise_error("model ", hardware_model)
            raise_error("hyper-v virtual machine ", hyper_v_virtual_machine)
            raise_error("vmware virtual machine ", vmware_virtual_machine)
            raise_error("virtualbox virtual machine ", virtualbox_virtual_machine)
            raise_error("virtual machine ", virtual_machine)
        End If
    End Sub
End Module
