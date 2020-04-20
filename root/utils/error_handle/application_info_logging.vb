
Option Explicit On
Option Infer Off
Option Strict On

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
                    ", under clr ",
                    clr_version,
                    ", source control current changeset id ",
                    source_control.current.id,
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
        raise_error("detail source control info as",
                    ", changeset id ",
                    source_control.current.id,
                    ", submitted by ",
                    source_control.current.user,
                    ", at ",
                    source_control.current.date,
                    ", comment ",
                    source_control.current.comment)
        raise_error("detail tf info as",
                    ", changeset id ",
                    tf_current_changeset_id,
                    ", submitted by ",
                    tf_current_changeset_user,
                    ", at ",
                    tf_current_changeset_date,
                    ", comment ",
                    tf_current_changeset_comment)
        raise_error("detail git info as",
                    ", trackable_version ",
                    gitver.current.trackable_version,
                    ", hash ",
                    gitver.current.hash,
                    ", submitted by ",
                    gitver.current.author,
                    ", at ",
                    gitver.current.author_date,
                    ", comment ",
                    gitver.current.subject,
                    ", detail comment ",
                    gitver.current.body,
                    ", diff against base ",
                    gitver.diff)
        raise_error("host os info as",
                    ", family ",
                    os.family,
                    ", windows major ",
                    os.windows_major,
                    ", windows ver ",
                    os.windows_ver,
                    ", full name ",
                    os.full_name,
                    ", platform ",
                    os.platform,
                    ", version ",
                    os.version)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
