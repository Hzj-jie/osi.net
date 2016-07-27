
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.device

Public Class text_herald_adapter_convertor
    Public Shared ReadOnly string_command As binder(Of _do_val_ref(Of String, command, Boolean), 
                                                       string_conversion_binder_protector)
    Public Shared ReadOnly command_string As binder(Of _do_val_ref(Of command, String, Boolean), 
                                                       string_conversion_binder_protector)

    Shared Sub New()
        string_command = New binder(Of _do_val_ref(Of String, command, Boolean), string_conversion_binder_protector)()
        command_string = New binder(Of _do_val_ref(Of command, String, Boolean), string_conversion_binder_protector)()
    End Sub

    Private Sub New()
    End Sub
End Class

<type_attribute()>
Public Class text_herald_adapter
    Inherits text_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal t As text)
        MyBase.New(t,
                   text_herald_adapter_convertor.command_string,
                   text_herald_adapter_convertor.string_command)
    End Sub
End Class
