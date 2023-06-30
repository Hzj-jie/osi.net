
Imports osi.root.connector
Imports osi.root.utt

Public Class static_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(static:=True),
                   If(isreleasebuild(),
                      expected_max_loops.release_build.static,
                      expected_max_loops.debug_build.static),
                   times:=5)
    End Sub
End Class
