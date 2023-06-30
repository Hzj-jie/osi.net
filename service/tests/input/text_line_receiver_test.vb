
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.input
Imports utt_case = osi.root.utt.case
Imports input_case = osi.service.input.case

Public Class text_line_receiver_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New text_line_receiver_case(), 1024 * 128)
    End Sub

    Private Class text_line_receiver_case
        Inherits utt_case

        Private ReadOnly r As text_line_receiver
        Private s As String

        Public Sub New()
            r = New text_line_receiver()
            AddHandler r.line, AddressOf line
        End Sub

        Private Sub line(ByVal s As String, ByRef ec As event_comb)
            assertion.equal(Me.s, s)
            assertion.is_null(ec)
        End Sub

        Public Overrides Function run() As Boolean
            s = rnd_ascii_display_chars(rnd_int(10, 100))
            Dim v As vector(Of input_case) = Nothing
            If assertion.is_true(s.keyboard_case(v)) AndAlso
               assertion.is_not_null(v) AndAlso
               assertion.is_false(v.empty()) Then
                For j As Int32 = 0 To v.size() - 1
                    r.receive(v(j))
                Next
            End If
            Return True
        End Function
    End Class
End Class
