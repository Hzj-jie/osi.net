
Imports System.Net
Imports osi.root.formation
Imports osi.service.device

Partial Public Class listener
    Public Shadows Class multiple_accepter
        Inherits dispenser(Of Byte(), IPEndPoint).multiple_accepter

        Private ReadOnly sources As const_array(Of IPEndPoint)

        Public Sub New(ByVal sources As const_array(Of IPEndPoint))
            MyBase.New(sources)
        End Sub

        Protected Overrides Function match(ByVal remote As IPEndPoint, ByVal source As IPEndPoint) As Boolean
            Return remote.match_endpoint(source)
        End Function
    End Class

    Public Class one_accepter
        Inherits dispenser(Of Byte(), IPEndPoint).accepter

        Private ReadOnly source As IPEndPoint

        Public Sub New(ByVal source As IPEndPoint)
            Me.source = source
        End Sub

        Public NotOverridable Overrides Function accept(ByVal remote As IPEndPoint) As Boolean
            If source Is Nothing Then
                Return True
            Else
                Return remote.match_endpoint(source)
            End If
        End Function
    End Class
End Class
