﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class bt(Of T)
    Partial Protected Friend Class node
        Private Shared Function left_rotate(ByVal a As node) As node
            assert(Not a Is Nothing)
            assert(a.has_right_child())
            Dim c As node = Nothing
            c = a.right_child()
            Dim d As node = Nothing
            d = c.left_child()
            a.replace_parent_subtree(c)
            a.clear_parent()
            c.replace_left(a)
            a.replace_right(d)

            a.debug_assert_structure()
            c.debug_assert_structure()

            Return c
        End Function

        Public Function left_rotate() As node
            Return left_rotate(Me)
        End Function

        Private Shared Function right_rotate(ByVal a As node) As node
            assert(Not a Is Nothing)
            assert(a.has_left_child())
            Dim b As node = Nothing
            b = a.left_child()
            Dim e As node = Nothing
            e = b.right_child()
            a.replace_parent_subtree(b)
            a.clear_parent()
            b.replace_right(a)
            a.replace_left(e)

            a.debug_assert_structure()
            b.debug_assert_structure()

            Return b
        End Function

        Public Function right_rotate() As node
            Return right_rotate(Me)
        End Function
    End Class
End Class
