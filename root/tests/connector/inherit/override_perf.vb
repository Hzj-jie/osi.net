
Imports osi.root.connector
Imports osi.root.utt

Public Class override_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(override:=True),
                   If(isreleasebuild(),
                      expected_max_loops.release_build.override,
                      expected_max_loops.debug_build.override),
                   times:=5)
    End Sub
End Class
