
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.iosys

Public Class keyboard_command_receiver
    Inherits keyboard_receiver

    Public Event ctrl_command(ByVal kc As Int32, ByRef ec As event_comb)
    Public Event alt_command(ByVal kc As Int32, ByRef ec As event_comb)
    Public Event ctrl_alt_command(ByVal kc As Int32, ByRef ec As event_comb)
    Public Event command(ByVal kc As Int32, ByVal ctrl As Boolean, ByVal alt As Boolean, ByRef ec As event_comb)

    Public Sub New(ByVal rs As keyboard_status)
        MyBase.New(rs)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Private Function raise_events(ByVal kc As Int32) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If rs.ctrl() Then
                                      RaiseEvent ctrl_command(kc, ec)
                                  End If
                                  Return If(ec Is Nothing, True, waitfor(ec)) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec Is Nothing OrElse ec.end_result() Then
                                      If rs.alt() Then
                                          RaiseEvent alt_command(kc, ec)
                                      End If
                                      Return If(ec Is Nothing, True, waitfor(ec)) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec Is Nothing OrElse ec.end_result() Then
                                      If rs.ctrl_alt() Then
                                          RaiseEvent ctrl_alt_command(kc, ec)
                                      End If
                                      Return If(ec Is Nothing, True, waitfor(ec)) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec Is Nothing OrElse ec.end_result() Then
                                      RaiseEvent command(kc, rs.ctrl(), rs.alt(), ec)
                                      Return If(ec Is Nothing, True, waitfor(ec)) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return (ec Is Nothing OrElse ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Overrides Function handle(ByVal action As action, ByVal kc As Int32, ByRef ec As event_comb) As Boolean
        If action = action.press AndAlso
           rs.ctrl_or_alt() Then
            ec = raise_events(kc)
            Return True
        Else
            Return False
        End If
    End Function
End Class
