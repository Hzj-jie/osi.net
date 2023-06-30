
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utils
Imports osi.service
Imports osi.service.input
Imports utt_case = osi.root.utt.case
Imports input_case = osi.service.input.case

Public Class char_receiver_test
    Inherits utt_case

    Private ReadOnly r As char_receiver
    Private c As Char

    Public Sub New()
        r = New char_receiver()
        AddHandler r.character, AddressOf receive_character
    End Sub

    Private Sub receive_character(ByVal c As Char, ByRef ec As event_comb)
        assertion.equal(Me.c, c)
        assertion.is_null(ec)
    End Sub

    Public Overrides Function run() As Boolean
        For i As Int32 = character.ascii_lower_bound To character.ascii_upper_bound
            c = Convert.ToChar(i)
            If Not Char.IsControl(c) Then
                Dim v As vector(Of input_case) = Nothing
                If assertion.is_true(c.keyboard_case(v)) AndAlso
                   assertion.is_not_null(v) AndAlso
                   assertion.is_false(v.empty()) Then
                    For j As Int32 = 0 To v.size() - 1
                        r.receive(v(j))
                    Next
                End If
            End If
        Next
        Return True
    End Function
End Class
