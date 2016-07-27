
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

<global_init(global_init_level.other)>
Friend Class application_info_logging
    Shared Sub New()
        raise_error(error_type.application,
                    "start error_handle for process ",
                    application_name,
                    ", ver ",
                    application_version,
                    ", process id ",
                    current_process.Id(),
                    ", tf current changeset id ",
                    tf_current_changeset_id,
                    ", built at ",
                    buildtime)
        raise_error("detail build info as",
                    ", time ",
                    buildtime,
                    ", machine ",
                    buildmachine,
                    ", user ",
                    builduser,
                    ", system ver ",
                    buildsysver,
                    ", processor ",
                    buildprocessor)
        raise_error("detail tfs info as",
                    ", changeset id ",
                    tf_current_changeset_id,
                    ", submitted by ",
                    tf_current_changeset_user,
                    ", at ",
                    tf_current_changeset_date,
                    ", comment ",
                    tf_current_changeset_comment)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
