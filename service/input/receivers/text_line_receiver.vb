
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

Public Class text_line_receiver
    Inherits keyboard_receiver

    Public Event line(ByVal s As String, ByRef ec As event_comb)
    Private ReadOnly q As vector(Of Char)

    Public Sub New(ByVal rs As keyboard_status)
        MyBase.New(rs)
        Me.q = New vector(Of Char)()
    End Sub

    Public Sub New()
        Me.New(New keyboard_status())
    End Sub

    Private Function to_line() As String
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        For i As Int32 = 0 To q.size() - 1
            r.Append(q(i))
        Next
        q.clear()
        Return Convert.ToString(r)
    End Function

    Protected Overrides Function handle(ByVal action As action,
                                        ByVal kc As Int32,
                                        ByRef ec As event_comb) As Boolean
        If action = action.press AndAlso
           Not rs.ctrl_or_alt() Then
            'treat alt or ctrl as command
            If kc = ConsoleKey.Enter Then
                RaiseEvent line(to_line(), ec)
                Return True
            ElseIf kc = ConsoleKey.Backspace Then
                q.pop_back()
                Return True
            Else
                Dim c As Char = character.null
                If kc.keycode_char(c,
                                   rs.caps_lock(),
                                   rs.num_lock(),
                                   rs.shift()) Then
                    q.emplace_back(c)
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function
End Class
