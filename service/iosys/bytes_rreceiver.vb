
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.commander

Public Class bytes_rreceiver(Of CASE_T)
    Inherits command_rreceiver(Of CASE_T)

    Private ReadOnly s As Func(Of Byte(), event_comb)

    Public Sub New(ByVal send As Func(Of Byte(), event_comb))
        MyBase.New()
        assert(Not send Is Nothing)
        Me.s = send
    End Sub

    Protected Overrides Function receive(ByVal i As command) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      Dim b() As Byte = Nothing
                                      b = bytes_serializer.to_bytes(i)
                                      ec = s(b)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
