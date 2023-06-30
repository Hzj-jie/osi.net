
Imports osi.root.procedure
Imports osi.root.connector

Public Class char_receiver
    Inherits keyboard_receiver

    Public Event character(ByVal c As Char, ByRef ec As event_comb)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal rs As keyboard_status)
        MyBase.New(rs)
    End Sub

    Protected Overrides Function handle(ByVal action As action,
                                        ByVal kc As Int32,
                                        ByRef ec As event_comb) As Boolean
        If action = action.press AndAlso
           Not rs.ctrl_or_alt() Then
            'treat alt or ctrl as command
            Dim c As Char = root.constants.character.null
            If kc.keycode_char(c,
                               rs.caps_lock(),
                               rs.num_lock(),
                               rs.shift()) Then
                RaiseEvent character(c, ec)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
End Class
