
Imports osi.root.connector
Imports osi.root.utt

Public Class impl_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(New inherit_case(impl:=True),
                   If(isreleasebuild(), expected_max_loops.release_build.impl, expected_max_loops.debug_build.impl),
                   times:=5)
    End Sub
End Class
