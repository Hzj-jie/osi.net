
Imports osi.root.connector
Imports osi.root.utt

Public Class base_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(base:=True),
                   If(isreleasebuild(), expected_max_loops.release_build.base, expected_max_loops.debug_build.base),
                   times:=5)
    End Sub
End Class
