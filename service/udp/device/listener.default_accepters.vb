
Imports System.Net
Imports osi.root.formation
Imports osi.service.device

Partial Public Class listener
    Public MustInherit Shadows Class multiple_accepter
        Inherits dispenser(Of Byte(), IPEndPoint).multiple_accepter

        Private ReadOnly sources As const_array(Of IPEndPoint)

        Public Sub New(ByVal sources As const_array(Of IPEndPoint), Optional ByVal always_accept As Boolean = False)
            MyBase.New(sources, always_accept)
        End Sub

        Protected Overrides Function match(ByVal remote As IPEndPoint, ByVal source As IPEndPoint) As Boolean
            Return remote.match_endpoint(source)
        End Function
    End Class

    Public MustInherit Class one_accepter
        Implements dispenser(Of Byte(), IPEndPoint).accepter

        Private ReadOnly source As IPEndPoint

        Public Sub New(ByVal source As IPEndPoint)
            Me.source = source
        End Sub

        Public Function accept(ByVal remote As IPEndPoint) As Boolean _
                              Implements dispenser(Of Byte(), IPEndPoint).accepter.accept
            If source Is Nothing Then
                Return True
            Else
                Return remote.match_endpoint(source)
            End If
        End Function

        Public MustOverride Sub received(ByVal buff As Byte(), ByVal remote As IPEndPoint) _
                                        Implements dispenser(Of Byte(), IPEndPoint).accepter.received
    End Class
End Class
