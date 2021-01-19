
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.service.transmitter

<type_attribute()>
Public Class text_herald_adapter
    Inherits text_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal t As text)
        MyBase.New(t)
    End Sub

    Public Shared Function [New](ByVal t As text) As herald
        Return New text_herald_adapter(t)
    End Function
End Class
