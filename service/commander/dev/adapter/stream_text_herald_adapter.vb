
Imports osi.root.utils
Imports osi.service.device
Imports Encoding = System.Text.Encoding

<type_attribute()>
Public Class stream_text_herald_adapter
    Inherits stream_text_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal t As stream_text, Optional ByVal enc As Encoding = Nothing)
        MyBase.New(t, enc, text_herald_adapter_convertor.command_string, text_herald_adapter_convertor.string_command)
    End Sub
End Class
