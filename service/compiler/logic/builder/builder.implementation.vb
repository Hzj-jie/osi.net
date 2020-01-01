
' This file is generated by builder_gen and definition.txt.
' Do not edit.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class builders

        Public Shared Function of_type(ByVal string_1 As String, ByVal uint_2 As UInt32) As type_builder_1
            Return New type_builder_1(string_1, uint_2)
        End Function

        Public NotInheritable Class type_builder_1

            Private ReadOnly string_1 As String
            Private ReadOnly uint_2 As UInt32

            Public Sub New(ByVal string_1 As String, ByVal uint_2 As UInt32)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                Me.uint_2 = uint_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("type")
                o.append(string_1)
                o.append(uint_2)
            End Sub
        End Class

        Public Shared Function of_append_slice(ByVal string_1 As String, ByVal string_2 As String) As append_slice_builder_2
            Return New append_slice_builder_2(string_1, string_2)
        End Function

        Public NotInheritable Class append_slice_builder_2

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("append_slice")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_cut(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As cut_builder_3
            Return New cut_builder_3(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class cut_builder_3

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("cut")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_cut_slice(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String) As cut_slice_builder_4
            Return New cut_slice_builder_4(string_1, string_2, string_3, string_4)
        End Function

        Public NotInheritable Class cut_slice_builder_4

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String
            Private ReadOnly string_4 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
                assert(Not string_4.null_or_whitespace())
                Me.string_4 = string_4
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("cut_slice")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
                o.append(string_4)
            End Sub
        End Class

        Public Shared Function of_clear(ByVal string_1 As String) As clear_builder_5
            Return New clear_builder_5(string_1)
        End Function

        Public NotInheritable Class clear_builder_5

            Private ReadOnly string_1 As String

            Public Sub New(ByVal string_1 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("clear")
                o.append(string_1)
            End Sub
        End Class

        Public Shared Function of_add(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As add_builder_6
            Return New add_builder_6(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class add_builder_6

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("add")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_subtract(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As subtract_builder_7
            Return New subtract_builder_7(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class subtract_builder_7

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("subtract")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_multiply(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As multiply_builder_8
            Return New multiply_builder_8(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class multiply_builder_8

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("multiply")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_power(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As power_builder_9
            Return New power_builder_9(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class power_builder_9

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("power")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_and(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As and_builder_10
            Return New and_builder_10(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class and_builder_10

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("and")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_or(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As or_builder_11
            Return New or_builder_11(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class or_builder_11

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("or")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_callee(ByVal string_1 As String, ByVal typed_parameters_2 As vector(Of pair(Of String, String)), ByVal paragraph_3 As Func(Of Boolean)) As callee_builder_12
            Return New callee_builder_12(string_1, typed_parameters_2, paragraph_3)
        End Function

        Public NotInheritable Class callee_builder_12

            Private ReadOnly string_1 As String
            Private ReadOnly typed_parameters_2 As vector(Of pair(Of String, String))
            Private ReadOnly paragraph_3 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal typed_parameters_2 As vector(Of pair(Of String, String)), ByVal paragraph_3 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not typed_parameters_2 Is Nothing)
                Me.typed_parameters_2 = typed_parameters_2
                assert(Not paragraph_3 Is Nothing)
                Me.paragraph_3 = paragraph_3
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("callee")
                o.append(string_1)
                o.append(typed_parameters_2)
                o.append("{")
                o.append(paragraph_3)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_caller(ByVal string_1 As String, ByVal string_2 As String, ByVal parameters_3 As vector(Of String)) As caller_builder_13
            Return New caller_builder_13(string_1, string_2, parameters_3)
        End Function

        Public NotInheritable Class caller_builder_13

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly parameters_3 As vector(Of String)

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal parameters_3 As vector(Of String))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not parameters_3 Is Nothing)
                Me.parameters_3 = parameters_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("caller")
                o.append(string_1)
                o.append(string_2)
                o.append(parameters_3)
            End Sub
        End Class

        Public Shared Function of_caller(ByVal string_1 As String, ByVal parameters_2 As vector(Of String)) As caller_builder_14
            Return New caller_builder_14(string_1, parameters_2)
        End Function

        Public NotInheritable Class caller_builder_14

            Private ReadOnly string_1 As String
            Private ReadOnly parameters_2 As vector(Of String)

            Public Sub New(ByVal string_1 As String, ByVal parameters_2 As vector(Of String))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not parameters_2 Is Nothing)
                Me.parameters_2 = parameters_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("caller")
                o.append(string_1)
                o.append(parameters_2)
            End Sub
        End Class

        Public Shared Function of_less(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As less_builder_15
            Return New less_builder_15(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class less_builder_15

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("less")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_more(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As more_builder_16
            Return New more_builder_16(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class more_builder_16

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("more")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_equal(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As equal_builder_17
            Return New equal_builder_17(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class equal_builder_17

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("equal")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_less_or_equal(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As less_or_equal_builder_18
            Return New less_or_equal_builder_18(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class less_or_equal_builder_18

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("less_or_equal")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_more_or_equal(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As more_or_equal_builder_19
            Return New more_or_equal_builder_19(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class more_or_equal_builder_19

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("more_or_equal")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_if(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean), ByVal paragraph_4 As Func(Of Boolean)) As if_builder_20
            Return New if_builder_20(string_1, paragraph_2, paragraph_4)
        End Function

        Public NotInheritable Class if_builder_20

            Private ReadOnly string_1 As String
            Private ReadOnly paragraph_2 As Func(Of Boolean)
            Private ReadOnly paragraph_4 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean), ByVal paragraph_4 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not paragraph_2 Is Nothing)
                Me.paragraph_2 = paragraph_2
                assert(Not paragraph_4 Is Nothing)
                Me.paragraph_4 = paragraph_4
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("if")
                o.append(string_1)
                o.append("{")
                o.append(paragraph_2)
                o.append("}")
                o.append("else")
                o.append("{")
                o.append(paragraph_4)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_if(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean)) As if_builder_21
            Return New if_builder_21(string_1, paragraph_2)
        End Function

        Public NotInheritable Class if_builder_21

            Private ReadOnly string_1 As String
            Private ReadOnly paragraph_2 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not paragraph_2 Is Nothing)
                Me.paragraph_2 = paragraph_2
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("if")
                o.append(string_1)
                o.append("{")
                o.append(paragraph_2)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_copy(ByVal string_1 As String, ByVal string_2 As String) As copy_builder_22
            Return New copy_builder_22(string_1, string_2)
        End Function

        Public NotInheritable Class copy_builder_22

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("copy")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_copy_const(ByVal string_1 As String, ByVal data_block_2 As data_block) As copy_const_builder_23
            Return New copy_const_builder_23(string_1, data_block_2)
        End Function

        Public NotInheritable Class copy_const_builder_23

            Private ReadOnly string_1 As String
            Private ReadOnly data_block_2 As data_block

            Public Sub New(ByVal string_1 As String, ByVal data_block_2 As data_block)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not data_block_2 Is Nothing)
                Me.data_block_2 = data_block_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("copy_const")
                o.append(string_1)
                o.append(data_block_2)
            End Sub
        End Class

        Public Shared Function of_define(ByVal string_1 As String, ByVal string_2 As String) As define_builder_24
            Return New define_builder_24(string_1, string_2)
        End Function

        Public NotInheritable Class define_builder_24

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("define")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_do_until(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean)) As do_until_builder_25
            Return New do_until_builder_25(string_1, paragraph_2)
        End Function

        Public NotInheritable Class do_until_builder_25

            Private ReadOnly string_1 As String
            Private ReadOnly paragraph_2 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not paragraph_2 Is Nothing)
                Me.paragraph_2 = paragraph_2
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("do_until")
                o.append(string_1)
                o.append("{")
                o.append(paragraph_2)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_do_while(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean)) As do_while_builder_26
            Return New do_while_builder_26(string_1, paragraph_2)
        End Function

        Public NotInheritable Class do_while_builder_26

            Private ReadOnly string_1 As String
            Private ReadOnly paragraph_2 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not paragraph_2 Is Nothing)
                Me.paragraph_2 = paragraph_2
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("do_while")
                o.append(string_1)
                o.append("{")
                o.append(paragraph_2)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_interrupt(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String) As interrupt_builder_27
            Return New interrupt_builder_27(string_1, string_2, string_3)
        End Function

        Public NotInheritable Class interrupt_builder_27

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("interrupt")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
            End Sub
        End Class

        Public Shared Function of_move(ByVal string_1 As String, ByVal string_2 As String) As move_builder_28
            Return New move_builder_28(string_1, string_2)
        End Function

        Public NotInheritable Class move_builder_28

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("move")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_divide(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String) As divide_builder_29
            Return New divide_builder_29(string_1, string_2, string_3, string_4)
        End Function

        Public NotInheritable Class divide_builder_29

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String
            Private ReadOnly string_4 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
                assert(Not string_4.null_or_whitespace())
                Me.string_4 = string_4
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("divide")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
                o.append(string_4)
            End Sub
        End Class

        Public Shared Function of_extract(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String) As extract_builder_30
            Return New extract_builder_30(string_1, string_2, string_3, string_4)
        End Function

        Public NotInheritable Class extract_builder_30

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String
            Private ReadOnly string_3 As String
            Private ReadOnly string_4 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String, ByVal string_3 As String, ByVal string_4 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
                assert(Not string_3.null_or_whitespace())
                Me.string_3 = string_3
                assert(Not string_4.null_or_whitespace())
                Me.string_4 = string_4
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("extract")
                o.append(string_1)
                o.append(string_2)
                o.append(string_3)
                o.append(string_4)
            End Sub
        End Class

        Public Shared Function of_return(ByVal string_1 As String) As return_builder_31
            Return New return_builder_31(string_1)
        End Function

        Public NotInheritable Class return_builder_31

            Private ReadOnly string_1 As String

            Public Sub New(ByVal string_1 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("return")
                o.append(string_1)
                o.append("*")
            End Sub
        End Class

        Public Shared Function of_return(ByVal string_1 As String, ByVal string_2 As String) As return_builder_32
            Return New return_builder_32(string_1, string_2)
        End Function

        Public NotInheritable Class return_builder_32

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("return")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_append(ByVal string_1 As String, ByVal string_2 As String) As append_builder_33
            Return New append_builder_33(string_1, string_2)
        End Function

        Public NotInheritable Class append_builder_33

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("append")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_not(ByVal string_1 As String, ByVal string_2 As String) As not_builder_34
            Return New not_builder_34(string_1, string_2)
        End Function

        Public NotInheritable Class not_builder_34

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("not")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_sizeof(ByVal string_1 As String, ByVal string_2 As String) As sizeof_builder_35
            Return New sizeof_builder_35(string_1, string_2)
        End Function

        Public NotInheritable Class sizeof_builder_35

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("sizeof")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_empty(ByVal string_1 As String, ByVal string_2 As String) As empty_builder_36
            Return New empty_builder_36(string_1, string_2)
        End Function

        Public NotInheritable Class empty_builder_36

            Private ReadOnly string_1 As String
            Private ReadOnly string_2 As String

            Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not string_2.null_or_whitespace())
                Me.string_2 = string_2
            End Sub

            Public Sub [to](ByVal o As writer)
                o.append("empty")
                o.append(string_1)
                o.append(string_2)
            End Sub
        End Class

        Public Shared Function of_while_then(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean)) As while_then_builder_37
            Return New while_then_builder_37(string_1, paragraph_2)
        End Function

        Public NotInheritable Class while_then_builder_37

            Private ReadOnly string_1 As String
            Private ReadOnly paragraph_2 As Func(Of Boolean)

            Public Sub New(ByVal string_1 As String, ByVal paragraph_2 As Func(Of Boolean))
                assert(Not string_1.null_or_whitespace())
                Me.string_1 = string_1
                assert(Not paragraph_2 Is Nothing)
                Me.paragraph_2 = paragraph_2
            End Sub

            Public Function [to](ByVal o As writer) As Boolean
                o.append("while_then")
                o.append(string_1)
                o.append("{")
                o.append(paragraph_2)
                o.append("}")
                Return True
            End Function
        End Class

        Public Shared Function of_stop() As stop_builder_38
            Return New stop_builder_38()
        End Function

        Public NotInheritable Class stop_builder_38

            Public Sub [to](ByVal o As writer)
                o.append("stop")
            End Sub
        End Class
    End Class
End Namespace
