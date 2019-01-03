
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class shift_test
    Inherits [case]

    Private Shared Function uint32_case() As Boolean
        Const b As UInt32 = &H11FF11FF      '0001 0001 1111 1111 0001 0001 1111 1111
        Const b_l_4 As UInt32 = &H1FF11FF1  '0001 1111 1111 0001 0001 1111 1111 0001
        Const b_l_5 As UInt32 = &H3FE23FE2  '0011 1111 1110 0010 0011 1111 1110 0010
        Const b_l_6 As UInt32 = &H7FC47FC4  '0111 1111 1100 0100 0111 1111 1100 0100
        Dim i As UInt32 = 0
        i = b
        i.left_shift(16)
        assertion.equal(i, b)
        i.right_shift(16)
        assertion.equal(i, b)
        i.left_shift(32)
        assertion.equal(i, b)
        i.right_shift(32)
        assertion.equal(i, b)

        i.left_shift(4)
        assertion.equal(i, b_l_4)
        i.left_shift(1)
        assertion.equal(i, b_l_5)
        i.left_shift(1)
        assertion.equal(i, b_l_6)

        i.right_shift(1)
        assertion.equal(i, b_l_5)
        i.right_shift(1)
        assertion.equal(i, b_l_4)
        i.right_shift(4)
        assertion.equal(i, b)

        Return True
    End Function

    Private Shared Function uint64_case() As Boolean
        '0001 0001 1111 1111 0001 0001 1111 1111 0001 0001 1111 1111 0001 0001 1111 1111
        Const b As UInt64 = &H11FF11FF11FF11FF
        '0001 1111 1111 0001 0001 1111 1111 0001 0001 1111 1111 0001 0001 1111 1111 0001
        Const b_l_4 As UInt64 = &H1FF11FF11FF11FF1
        '0011 1111 1110 0010 0011 1111 1110 0010 0011 1111 1110 0010 0011 1111 1110 0010
        Const b_l_5 As UInt64 = &H3FE23FE23FE23FE2
        '0111 1111 1100 0100 0111 1111 1100 0100 0111 1111 1100 0100 0111 1111 1100 0100
        Const b_l_6 As UInt64 = &H7FC47FC47FC47FC4
        Dim i As UInt64 = 0
        i = b
        i.left_shift(16)
        assertion.equal(i, b)
        i.right_shift(16)
        assertion.equal(i, b)
        i.left_shift(32)
        assertion.equal(i, b)
        i.right_shift(32)
        assertion.equal(i, b)
        i.left_shift(48)
        assertion.equal(i, b)
        i.right_shift(48)
        assertion.equal(i, b)
        i.left_shift(64)
        assertion.equal(i, b)
        i.right_shift(64)
        assertion.equal(i, b)

        i.left_shift(4)
        assertion.equal(i, b_l_4)
        i.left_shift(1)
        assertion.equal(i, b_l_5)
        i.left_shift(1)
        assertion.equal(i, b_l_6)

        i.right_shift(1)
        assertion.equal(i, b_l_5)
        i.right_shift(1)
        assertion.equal(i, b_l_4)
        i.right_shift(4)
        assertion.equal(i, b)

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return uint32_case() AndAlso
               uint64_case()
    End Function
End Class
