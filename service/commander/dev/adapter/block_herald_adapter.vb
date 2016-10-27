
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.service.transmitter

Public Class block_herald_adapter_convertor
    Public Shared ReadOnly bytes_command As binder(Of _do_val_ref(Of Byte(), command, Boolean), 
                                                      bytes_conversion_binder_protector)
    Public Shared ReadOnly command_bytes As binder(Of _do_val_ref(Of command, Byte(), Boolean), 
                                                      bytes_conversion_binder_protector)

    Public Shared Function convert(ByVal i() As Byte, ByRef o As command) As Boolean
        ' FIXME: the status of command.from_bytes cannot return to respond
        '        without breaking the procedure
        o = New command()
        If Not o.from_bytes(i) Then
            o.clear()
            assert(o.empty())
        End If
        Return True
    End Function

    Shared Sub New()
        bytes_command = New binder(Of _do_val_ref(Of Byte(), command, Boolean), bytes_conversion_binder_protector)()
        bytes_command.set_local(AddressOf convert)
    End Sub

    Private Sub New()
    End Sub
End Class

<type_attribute()>
Public Class block_herald_adapter
    Inherits block_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal b As block)
        MyBase.New(b,
                   block_herald_adapter_convertor.command_bytes,
                   block_herald_adapter_convertor.bytes_command)
    End Sub

    Public Shared Function [New](ByVal b As block) As block_herald_adapter
        Return New block_herald_adapter(b)
    End Function
End Class
