
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' generic type should have no performance impact on allocating.
Public NotInheritable Class generic_ctor_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 100000000)
    End Function

    Public Sub New()
        MyBase.New(R(New gen_0_case()), R(New gen_1_case()), R(New gen_2_case()), R(New gen_3_case()))
    End Sub

    Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1, 1, 1, 1}, i, j)
    End Function

    Private NotInheritable Class gen_0
    End Class

    Private NotInheritable Class gen_1(Of T)
        Private Shared ReadOnly x As T = alloc(Of T)()
    End Class

    Private NotInheritable Class gen_2(Of T, T2)
        Private Shared ReadOnly x As T = alloc(Of T)()
        Private Shared ReadOnly y As T2 = alloc(Of T2)()
    End Class

    Private NotInheritable Class gen_3(Of T, T2, T3)
        Private Shared ReadOnly x As T = alloc(Of T)()
        Private Shared ReadOnly y As T2 = alloc(Of T2)()
        Private Shared ReadOnly z As T3 = alloc(Of T3)()
    End Class

    Private NotInheritable Class gen_0_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As gen_0 = New gen_0()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class gen_1_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As gen_1(Of Int32) = New gen_1(Of Int32)()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class gen_2_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As gen_2(Of Int32, Int32) = New gen_2(Of Int32, Int32)()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class gen_3_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As gen_3(Of Int32, Int32, Int32) = New gen_3(Of Int32, Int32, Int32)()
            Return x IsNot Nothing
        End Function
    End Class
End Class
