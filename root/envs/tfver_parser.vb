
Imports osi.root.connector
Imports osi.root.constants

Public Module _tfver_parser
    Public ReadOnly tf_latest_changeset_id As String = source_control.unknown_value
    Public ReadOnly tf_latest_changeset_user As String = source_control.unknown_value
    Public ReadOnly tf_latest_changeset_date As String = source_control.unknown_value
    Public ReadOnly tf_latest_changeset_comment As String = source_control.unknown_value

    Public ReadOnly tf_current_changeset_id As String = source_control.unknown_value
    Public ReadOnly tf_current_changeset_user As String = source_control.unknown_value
    Public ReadOnly tf_current_changeset_date As String = source_control.unknown_value
    Public ReadOnly tf_current_changeset_comment As String = source_control.unknown_value

    Private Function next_token(ByRef i As String, ByRef s As String) As Boolean
        While strsep(i, s, i, character.blank)
            If Not String.IsNullOrEmpty(s) Then
                Return True
            End If
        End While
        Return False
    End Function

    Sub New()
        Dim t As String = Nothing
        copy(t, tf_latest_changeset)
        If next_token(t, tf_latest_changeset_id) Then
            If next_token(t, tf_latest_changeset_user) Then
                If next_token(t, tf_latest_changeset_date) Then
                    If Not String.IsNullOrEmpty(t) Then
                        tf_latest_changeset_comment = t.Trim()
                    End If
                End If
            End If
        End If

        copy(t, tf_current_changeset)
        If next_token(t, tf_current_changeset_id) Then
            If next_token(t, tf_current_changeset_user) Then
                If next_token(t, tf_current_changeset_date) Then
                    If Not String.IsNullOrEmpty(t) Then
                        tf_current_changeset_comment = t.Trim()
                    End If
                End If
            End If
        End If
    End Sub
End Module
