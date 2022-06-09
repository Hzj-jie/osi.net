
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.commander

'the design cannot work with one connection
Public Class rreceiver(Of CASE_T)
    Inherits command_rreceiver(Of CASE_T)

    Private ReadOnly q As target_questioner

    Private Shared Function create_target_questioner(ByVal name As String, ByVal q As questioner) As target_questioner
        assert(Not name.null_or_empty())
        Return target_questioner.ctor(name, q)
    End Function

    Public Sub New(ByVal name As String, ByVal q As questioner)
        MyBase.New()
        Me.q = create_target_questioner(name, q)
    End Sub

    Protected Overrides Function receive(ByVal i As command) As event_comb
        Return q(i,
                 Function() As Boolean
                     Return True
                 End Function)
    End Function
End Class
