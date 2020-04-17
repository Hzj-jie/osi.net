﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.connector
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
            Return loosen_bound({1, 1}, i, j)
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
            Return loosen_bound({1, 1}, i, j)
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
            Return loosen_bound({1, 1}, i, j)
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
            Return loosen_bound({1, 1}, i, j)
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

    Public NotInheritable Class big_int_BigInteger_gcd_perf
        Inherits big_int_BigInteger_perf_comparison

        Public Sub New()
            MyBase.New(Sub(ByVal i As BigInteger, ByVal j As BigInteger)
                           BigInteger.GreatestCommonDivisor(BigInteger.Abs(i), BigInteger.Abs(j))
                       End Sub,
                       Sub(ByVal i As big_int, ByVal j As big_int)
                           big_uint.gcd(i.abs_big_uint(), j.abs_big_uint())
                       End Sub,
                       10000)
        End Sub

        Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({1, 1}, i, j)
        End Function
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

            Private Shared ReadOnly samples As vector(Of big_int)
            Private Shared ReadOnly samples_BigInteger As vector(Of BigInteger)
            Private ReadOnly e1 As Action(Of big_int, big_int)
            Private ReadOnly e2 As Action(Of BigInteger, BigInteger)
            Private ReadOnly tc As debug_thread_checker
            Private index As Int64

            Shared Sub New()
                samples = New vector(Of big_int)()
                For i As Int32 = 0 To 792
                    samples.emplace_back(big_int.random())
                Next
                samples_BigInteger = samples.map(Function(ByVal i As big_int) As BigInteger
                                                     Return i.as_BigInteger()
                                                 End Function)
            End Sub

            Private Sub New(ByVal e1 As Action(Of big_int, big_int), ByVal e2 As Action(Of BigInteger, BigInteger))
                assert(Not e1 Is Nothing OrElse Not e2 Is Nothing)
                assert(e1 Is Nothing OrElse e2 Is Nothing)
                Me.e1 = e1
                Me.e2 = e2
                tc = New debug_thread_checker()
                index = 0
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
                    i = next_random_BigInteger()
                    j = next_random_BigInteger()
                    e2(i, j)
                Else
                    Dim i As big_int = Nothing
                    Dim j As big_int = Nothing
                    i = next_random()
                    j = next_random()
                    e1(i, j)
                End If

                Return True
            End Function

            Private Function next_random() As big_int
                tc.assert()
                index += 1
                Return samples.modget(index).CloneT()
            End Function

            Private Function next_random_BigInteger() As BigInteger
                tc.assert()
                index += 1
                Return samples_BigInteger.modget(index)
            End Function
        End Class
    End Class

    Private Sub New()
    End Sub
End Class
