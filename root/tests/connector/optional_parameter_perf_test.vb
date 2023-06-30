
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public NotInheritable Class optional_parameter_perf_test
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(r(New overload_case()), r(New optional_parameter_case()))
    End Sub

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 1000000000)
    End Function

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({401, 414}, i, j)
    End Function

    Private NotInheritable Class overload_case
        Inherits [case]

        Private Shared Sub f(ByVal x As Int32)
        End Sub

        Private Shared Sub f()
            f(1)
        End Sub

        Public Overrides Function run() As Boolean
            f()
            Return True
        End Function
    End Class

    Private NotInheritable Class optional_parameter_case
        Inherits [case]

        Private Shared Sub f(Optional ByVal x As Int32 = 1)
        End Sub

        Public Overrides Function run() As Boolean
            f()
            Return True
        End Function
    End Class
End Class
