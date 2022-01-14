
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Management
Imports osi.root.connector
Imports osi.root.constants

<global_init(global_init_level.other)>
Public Module _hardware
    Public ReadOnly hardware_manufacturer As String = calculate_hardware_manufacturer()
    Public ReadOnly hardware_model As String = calculate_hardware_model()
    Public ReadOnly hyper_v_virtual_machine As Boolean =
        strsame(hardware_manufacturer, "microsoft corporation", False) AndAlso
        strcontains(hardware_model, "virtual", False)
    Public ReadOnly vmware_virtual_machine As Boolean = strcontains(hardware_manufacturer, "vmware", False)
    Public ReadOnly virtualbox_virtual_machine As Boolean = strsame(hardware_model, "virtualbox", False)
    Public ReadOnly virtual_machine As Boolean =
        hyper_v_virtual_machine OrElse
        vmware_virtual_machine OrElse
        virtualbox_virtual_machine
    Public ReadOnly report_hardware_info As Boolean = env_bool(env_keys("report", "hardware", "info"))

    Private Function calculate_hardware_manufacturer() As String
        If envs.mono Then
            Return "Mono"
        End If
        Return search_management_object("Manufacturer")
    End Function

    Private Function calculate_hardware_model() As String
        If envs.mono Then
            Return "Mono"
        End If
        Return search_management_object("Model")
    End Function

    Private Function search_management_object(ByVal item_name As String) As String
        Using searcher As New ManagementObjectSearcher("select * from Win32_ComputerSystem")
            Using items As ManagementObjectCollection = searcher.Get()
                For Each item As ManagementBaseObject In items
                    Using item
                        Dim s As String = item(item_name).ToString()
                        If Not String.IsNullOrEmpty(s) Then
                            Return s
                        End If
                    End Using
                Next
            End Using
        End Using
        Return ""
    End Function

    Private Sub init()
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
