
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.service.transmitter

<type_attribute()>
Public Class block_herald_adapter
    Inherits block_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal b As block)
        MyBase.New(b)
    End Sub

    Public Shared Function [New](ByVal b As block) As block_herald_adapter
        Return New block_herald_adapter(b)
    End Function
End Class
