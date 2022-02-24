
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.selector

Partial Public Class sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Private Class default_accepter
        Inherits dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter

        Private ReadOnly remote As const_pair(Of ADDRESS_T, PORT_T)

        Public Sub New(ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
            assert(Not remote Is Nothing)
            Me.remote = remote
        End Sub

        Public Overrides Function accept(ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) As Boolean
            Return Me.remote.CompareTo(remote) = 0
        End Function
    End Class
End Class
