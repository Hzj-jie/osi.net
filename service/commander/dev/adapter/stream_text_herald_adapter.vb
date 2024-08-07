
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.service.transmitter
Imports Encoding = System.Text.Encoding

<type_attribute()>
Public Class stream_text_herald_adapter
    Inherits stream_text_dev_T_adapter(Of command)
    Implements herald

    Public Sub New(ByVal t As stream_text, Optional ByVal enc As Encoding = Nothing)
        MyBase.New(t, enc)
    End Sub

    Public Shared Function [New](ByVal t As stream_text) As stream_text_herald_adapter
        Return New stream_text_herald_adapter(t)
    End Function
End Class
