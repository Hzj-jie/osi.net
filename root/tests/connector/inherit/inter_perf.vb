
Imports osi.root.connector
Imports osi.root.utt

Public Class inter_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(inter:=True),
                   If(isreleasebuild(), expected_max_loops.release_build.inter, expected_max_loops.debug_build.inter),
                   times:=5)
    End Sub
End Class
