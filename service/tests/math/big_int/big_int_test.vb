﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

Public NotInheritable Class big_int_test
    Inherits chained_case_wrapper

    Public Sub New()
        'the following tests depend on the previous ones
        MyBase.New(big_int_add_sub_multiply_case.create_case(),
                   big_int_shift_case.create_case(),
                   big_int_left_shift_multiply_case.create_case(),
                   big_int_right_shift_divide_case.create_case(),
                   big_int_divide_multiply_case.create_case(),
                   big_int_power_divide_case.create_case(),
                   big_int_power_extract_case.create_case(),
                   big_int_str_case.create_case(),
                   big_int_bytes_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_add_sub_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_add_sub_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_shift_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_shift_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_left_shift_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_left_shift_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_right_shift_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_right_shift_divide_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_divide_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_divide_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_power_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_power_divide_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_power_extract_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_power_extract_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_str_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_str_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_int_bytes_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_bytes_case.create_case())
    End Sub
End Class

<test>
Public NotInheritable Class big_int_other_tests
    <test>
    Private Shared Sub power_1()
        Dim i As big_int = Nothing
        i = big_int.random()
        assertion.equal(i ^ 1, i)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub power_7_7()
        Dim i As big_int = Nothing
        i = New big_int(7)
        For j As Int32 = 0 To 7 - 1
            i = i ^ 7
        Next
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub BigInteger_power_7_7()
        Dim i As BigInteger = Nothing
        i = New BigInteger(7)
        For j As Int32 = 0 To 7 - 1
            i = BigInteger.Pow(i, 7)
        Next
    End Sub

    Private Sub New()
    End Sub
End Class