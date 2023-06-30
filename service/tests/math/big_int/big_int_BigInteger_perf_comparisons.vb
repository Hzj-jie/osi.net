
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.math

Public NotInheritable Class big_int_BigInteger_perf_comparisons
    Public NotInheritable Class big_int_BigInteger_add_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            Me.New(default_size)
        End Sub

        Public Sub New(ByVal size As UInt32)
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i += j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i += j
                       End Sub,
                       size)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({653, 1296}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_add_large_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New big_int_BigInteger_add_perf(10000000))
        End Sub
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
            Return loosen_bound({809, 1950}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_multiply_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            Me.New(default_size)
        End Sub

        Public Sub New(ByVal size As UInt64)
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i *= j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i *= j
                       End Sub,
                       size)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({544, 956}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_multiply_large_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New big_int_BigInteger_multiply_perf(1000000))
        End Sub
    End Class

    Public NotInheritable Class big_int_BigInteger_divide_perf
        Inherits big_int_BigInteger_perf_comparison


        Public Sub New()
            Me.New(default_size)
        End Sub

        Public Sub New(ByVal size As UInt64)
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           i /= j
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i \= j
                       End Sub,
                       size)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({267, 941}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_divide_large_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New big_int_BigInteger_divide_perf(1000000))
        End Sub
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
            Return loosen_bound({267, 865}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_power_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.Pow(i, CInt((BigInteger.Abs(j) Mod 127) + 1))
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           i ^= (j.unsigned_ref().lowest_uint32() Mod 127) + 1
                       End Sub,
                       100)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({157, 303}, i, j)
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
            Return loosen_bound({33866961264, 15504057375}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_gcd_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            Me.New(10000)
        End Sub

        Public Sub New(ByVal size As UInt32)
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.GreatestCommonDivisor(
                               BigInteger.Abs(i * j * multiplier(CUInt(BigInteger.Abs(j) Mod max_uint32))),
                               BigInteger.Abs(j * multiplier(CUInt(BigInteger.Abs(j) Mod max_uint32))))
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           big_uint.gcd((i * j * multiplier(j.unsigned_ref().lowest_uint32())).unsigned_ref(),
                                        (j * multiplier(j.unsigned_ref().lowest_uint32())).unsigned_ref())
                       End Sub,
                       size)
        End Sub

        Private Shared Function multiplier(ByVal i As UInt32) As UInt32
            Return (CULng(primes.select_precalculated(i And max_uint16)) *
                    primes.select_precalculated((i >> bit_count_in_byte) >> bit_count_in_byte)).low_uint32()
        End Function

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({256, 729}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_gcd_edge_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            Me.New(10000)
        End Sub

        Public Sub New(ByVal size As UInt32)
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.GreatestCommonDivisor(BigInteger.Abs(i), BigInteger.Abs(j))
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           big_uint.gcd(i.unsigned_ref(), j.unsigned_ref())
                       End Sub,
                       size)
        End Sub

        Private Shared Function multiplier(ByVal i As UInt32) As UInt32
            Return (CULng(primes.select_precalculated(i And max_uint16)) *
                    primes.select_precalculated((i >> bit_count_in_byte) >> bit_count_in_byte)).low_uint32()
        End Function

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({154, 3308}, i, j)
        End Function
    End Class

    Public NotInheritable Class big_int_BigInteger_gcd_large_perf
        Inherits commandline_specified_case_wrapper

        Public Sub New()
            MyBase.New(New big_int_BigInteger_gcd_perf(100000))
        End Sub
    End Class

    Public MustInherit Class big_int_BigInteger_perf_comparison
        Inherits performance_comparison_case_wrapper

        Protected Const default_size As UInt64 = 100000

        Public Sub New(ByVal e1 As Action(Of BigInteger, BigInteger), ByVal e2 As Action(Of big_int, big_int))
            Me.New(e1, e2, default_size)
        End Sub

        Public Sub New(ByVal e1 As Action(Of BigInteger, BigInteger),
                       ByVal e2 As Action(Of big_int, big_int),
                       ByVal size As UInt64)
            MyBase.New(r(New run_case(e1), size),
                       r(New run_case(e2), size))
        End Sub

        Private Shared Function r(ByVal c As [case], ByVal size As UInt64) As [case]
            Return repeat(c, size)
        End Function

        Private NotInheritable Class run_case
            Inherits [case]

            Private Shared ReadOnly big_ints As samples(Of big_int)
            Private Shared ReadOnly BigIntegers As samples(Of BigInteger)
            Private ReadOnly e1 As Action(Of big_int, big_int)
            Private ReadOnly e2 As Action(Of BigInteger, BigInteger)

            Shared Sub New()
                big_ints = samples.of(AddressOf big_int.random, 792)
                BigIntegers = big_ints.map(Function(ByVal i As big_int) As BigInteger
                                               Return i.as_BigInteger()
                                           End Function)
            End Sub

            Private Sub New(ByVal e1 As Action(Of big_int, big_int), ByVal e2 As Action(Of BigInteger, BigInteger))
                assert(Not e1 Is Nothing OrElse Not e2 Is Nothing)
                assert(e1 Is Nothing OrElse e2 Is Nothing)
                Me.e1 = e1
                Me.e2 = e2
            End Sub

            Public Sub New(ByVal e As Action(Of big_int, big_int))
                Me.New(e, Nothing)
            End Sub

            Public Sub New(ByVal e As Action(Of BigInteger, BigInteger))
                Me.New(Nothing, e)
            End Sub

            Public Overrides Function run() As Boolean
                If e1 Is Nothing Then
                    Dim i As BigInteger = Nothing
                    Dim j As BigInteger = Nothing
                    i = BigIntegers.next()
                    j = BigIntegers.next()
                    e2(i, j)
                Else
                    Dim i As big_int = Nothing
                    Dim j As big_int = Nothing
                    i = big_ints.next()
                    j = big_ints.next()
                    e1(i, j)
                End If

                Return True
            End Function
        End Class
    End Class

    Private Sub New()
    End Sub
End Class
