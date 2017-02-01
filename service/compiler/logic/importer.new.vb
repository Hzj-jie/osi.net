﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class importer
        Private Structure place_holder
        End Structure

        Private Function new_type(ByVal p1 As String, ByVal p2 As UInt32) As type
            Return New type(types, p1, p2)
        End Function

        Private Function new_append_slice(ByVal p1 As String, ByVal p2 As String) As append_slice
            Return New append_slice(types, p1, p2)
        End Function

        Private Function new_cut(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As cut
            Return New cut(types, p1, p2, p3)
        End Function

        Private Function new_cut_slice(ByVal p1 As String,
                                       ByVal p2 As String,
                                       ByVal p3 As String,
                                       ByVal p4 As String) As cut_slice
            Return New cut_slice(types, p1, p2, p3, p4)
        End Function

        Private Function new_clear(ByVal p As String) As clear
            Return New clear(types, p)
        End Function

        Private Function new_add(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As add
            Return New add(types, p1, p2, p3)
        End Function

        Private Function new_substract(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As subtract
            Return New subtract(types, p1, p2, p3)
        End Function

        Private Function new_power(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As power
            Return New power(types, p1, p2, p3)
        End Function

        Private Function new_and(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As [and]
            Return New [and](types, p1, p2, p3)
        End Function

        Private Function new_or(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As [or]
            Return New [or](types, p1, p2, p3)
        End Function

        Private Function new_callee(ByVal p1 As String,
                                    ByVal p2 As vector(Of pair(Of String, String)),
                                    ByVal p3 As paragraph) As callee
            Return New callee(anchors, p1, unique_ptr.[New](+p2), unique_ptr.[New](p3))
        End Function

        Private Function new_caller(ByVal p1 As String, ByVal p2 As String, ByVal p3 As vector(Of String)) As caller
            Return New caller(anchors, p1, p2, +p3)
        End Function

        Private Function new_caller(ByVal p1 As String, ByVal p2 As vector(Of String)) As caller
            Return New caller(anchors, p1, +p2)
        End Function

        Private Function new_less(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As less
            Return New less(types, p1, p2, p3)
        End Function

        Private Function new_more(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As more
            Return New more(types, p1, p2, p3)
        End Function

        Private Function new_equal(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As equal
            Return New equal(types, p1, p2, p3)
        End Function

        Private Function new_less_or_equal(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As less_or_equal
            Return New less_or_equal(types, p1, p2, p3)
        End Function

        Private Function new_more_or_equal(ByVal p1 As String, ByVal p2 As String, ByVal p3 As String) As more_or_equal
            Return New more_or_equal(types, p1, p2, p3)
        End Function

        Private Function new_if(ByVal p1 As String,
                                ByVal p2 As paragraph,
                                ByVal p3 As place_holder,
                                ByVal p4 As paragraph) As condition
            Return New condition(p1, unique_ptr.[New](p2), unique_ptr.[New](p4))
        End Function

        Private Function new_if(ByVal p1 As String, ByVal p2 As paragraph) As condition
            Return New condition(p1, unique_ptr.[New](p2))
        End Function

        Private Function new_copy(ByVal p1 As String, ByVal p2 As String) As copy
            Return New copy(types, p1, p2)
        End Function

        Private Function new_copy_const(ByVal p1 As String, ByVal p2 As data_block) As copy_const
            Return New copy_const(types, p1, unique_ptr.[New](p2))
        End Function

        Private Function new_define(ByVal p1 As String, ByVal p2 As String) As define
            Return New define(p1, p2)
        End Function

        Private Function new_do_until(ByVal p1 As String, ByVal p2 As paragraph) As do_until
            Return New do_until(p1, unique_ptr.[New](p2))
        End Function

        Private Function new_do_while(ByVal p1 As String, ByVal p2 As paragraph) As do_while
            Return New do_while(p1, unique_ptr.[New](p2))
        End Function

        Private Function new_extern_function(ByVal p1 As String,
                                             ByVal p2 As String,
                                             ByVal p3 As String) As extern_function
            Return New extern_function(types, functions, p1, p2, p3)
        End Function

        Private Function new_move(ByVal p1 As String, ByVal p2 As String) As move
            Return New move(types, p1, p2)
        End Function

        Private Function new_divide(ByVal p1 As String,
                                    ByVal p2 As String,
                                    ByVal p3 As String,
                                    ByVal p4 As String) As divide
            Return New divide(types, p1, p2, p3, p4)
        End Function

        Private Function new_extract(ByVal p1 As String,
                                     ByVal p2 As String,
                                     ByVal p3 As String,
                                     ByVal p4 As String) As extract
            Return New extract(types, p1, p2, p3, p4)
        End Function

        Private Function new_return(ByVal p1 As String, ByVal p2 As String) As [return]
            Return New [return](anchors, p1, p2)
        End Function

        Private Function new_return(ByVal p As String) As [return]
            Return New [return](anchors, p)
        End Function

        Private Function new_append(ByVal p1 As String, ByVal p2 As String) As append
            Return New append(types, p1, p2)
        End Function

        Private Function new_not(ByVal p1 As String, ByVal p2 As String) As [not]
            Return New [not](types, p1, p2)
        End Function

        Private Function new_sizeof(ByVal p1 As String, ByVal p2 As String) As sizeof
            Return New sizeof(types, p1, p2)
        End Function

        Private Function new_empty(ByVal p1 As String, ByVal p2 As String) As empty
            Return New empty(types, p1, p2)
        End Function

        Private Function new_while_then(ByVal p1 As String, ByVal p2 As paragraph) As while_then
            Return New while_then(p1, unique_ptr.[New](p2))
        End Function
    End Class
End Namespace
