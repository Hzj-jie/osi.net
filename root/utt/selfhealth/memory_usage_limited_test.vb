
Imports osi.root.envs

Public Class memory_usage_limited_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(memory_usage_limited(New failure_case(), total_physical_memory() + 1),
                   memory_usage_limited(New failure_case(), total_virtual_memory() + 1))
    End Sub

    Private Class failure_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return assertion.is_true(False, "should never called.")
        End Function
    End Class
End Class
