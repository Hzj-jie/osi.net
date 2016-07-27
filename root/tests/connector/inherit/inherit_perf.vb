
Imports osi.root.connector
Imports osi.root.utt

Public Class inherit_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(inherit:=True),
                   If(isreleasebuild(),
                      expected_max_loops.release_build.inherit,
                      expected_max_loops.debug_build.inherit),
                   times:=5)
    End Sub
End Class
