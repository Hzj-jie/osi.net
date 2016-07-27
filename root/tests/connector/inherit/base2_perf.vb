
Imports osi.root.connector
Imports osi.root.utt

Public Class base2_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(base2:=True),
                   If(isreleasebuild(), expected_max_loops.release_build.base2, expected_max_loops.debug_build.base2),
                   times:=5)
    End Sub
End Class
