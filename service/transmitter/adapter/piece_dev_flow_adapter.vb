
Imports osi.root.utils

<type_attribute()>
Public Class piece_dev_flow_adapter
    Inherits block_flow_adapter

    Public Sub New(ByVal p As piece_dev)
        MyBase.New(New piece_dev_block_adapter(p))
    End Sub

    Public Shared Shadows Function [New](ByVal p As piece_dev) As flow
        Return New piece_dev_flow_adapter(p)
    End Function
End Class
