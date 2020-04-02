
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Public NotInheritable Class big_int_BigInteger_perf_comparisons
    Public NotInheritable Class big_int_BigInteger_add_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i += j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i += j
                       End Sub)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_sub_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i -= j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i -= j
                       End Sub)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_multiply_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i *= j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i *= j
                       End Sub)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_divide_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i /= j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i \= j
                       End Sub)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_modulus_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i = i Mod j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i = i Mod j
                       End Sub)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_power_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.Pow(i, CInt((BigInteger.Abs(j) Mod 127) + 1))
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i.power(j.set_positive().modulus(127) + 1)
                       End Sub,
                       100)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_io_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.Parse(i.ToString())
                           BigInteger.Parse(j.ToString())
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           big_int.parse(i.str())
                           big_int.parse(j.str())
                       End Sub,
                       10000)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
    End Class

    Public MustInherit Class big_int_BigInteger_perf_comparison
        Inherits performance_comparison_case_wrapper

        Public Sub New(ByVal e1 As Action(Of BigInteger, BigInteger), ByVal e2 As Action(Of big_int, big_int))
            Me.New(e1, e2, 100000)
        End Sub

        Public Sub New(ByVal e1 As Action(Of BigInteger, BigInteger),
                       ByVal e2 As Action(Of big_int, big_int),
                       ByVal size As UInt64)
            MyBase.New(r(New run_case(Sub(ByVal i As big_int, ByVal j As big_int)
                                          e1(i.as_BigInteger(), j.as_BigInteger())
                                      End Sub),
                         size),
                       r(New run_case(Sub(ByVal i As big_int, ByVal j As big_int)
                                          i.as_BigInteger()
                                          j.as_BigInteger()
                                          e2(i, j)
                                      End Sub),
                         size))
        End Sub

        Private Shared Function r(ByVal c As [case], ByVal size As UInt64) As [case]
            Return repeat(c, size)
        End Function

        Private NotInheritable Class run_case
            Inherits [case]

            Private ReadOnly e As Action(Of big_int, big_int)

            Public Sub New(ByVal e As Action(Of big_int, big_int))
                assert(Not e Is Nothing)
                Me.e = e
            End Sub

            Public Overrides Function run() As Boolean
                Dim i As big_int = Nothing
                Dim j As big_int = Nothing
                i = big_int.random()
                j = big_int.random()
                e(i, j)
                Return True
            End Function
        End Class
    End Class

    Private Sub New()
    End Sub
End Class
