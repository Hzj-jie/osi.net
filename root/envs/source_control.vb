
Public NotInheritable Class source_control
    Public Const unknown_value As String = "UNKNOWN"

    Public Class commit_info
        Public ReadOnly id As String
        Public ReadOnly user As String
        Public ReadOnly [date] As String
        Public ReadOnly comment As String

        Friend Sub New(ByVal id As String, ByVal user As String, ByVal [date] As String, ByVal comment As String)
            Me.id = id
            Me.user = user
            Me.date = [date]
            Me.comment = comment
        End Sub
    End Class

    Public Shared ReadOnly latest As commit_info
    Public Shared ReadOnly current As commit_info

    Shared Sub New()
        If tf_latest_changeset_id = unknown_value Then
            latest = New commit_info(gitver.latest.short_hash,
                                     gitver.latest.author,
                                     gitver.latest.author_date,
                                     gitver.latest.subject)
        Else
            latest = New commit_info(tf_latest_changeset_id,
                                     tf_latest_changeset_user,
                                     tf_latest_changeset_date,
                                     tf_latest_changeset_comment)
        End If
        If tf_current_changeset_id = unknown_value Then
            current = New commit_info(gitver.current.short_hash,
                                      gitver.current.author,
                                      gitver.current.author_date,
                                      gitver.current.subject)
        Else
            current = New commit_info(tf_current_changeset_id,
                                      tf_current_changeset_user,
                                      tf_current_changeset_date,
                                      tf_current_changeset_comment)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
