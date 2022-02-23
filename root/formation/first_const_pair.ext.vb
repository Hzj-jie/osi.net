﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public NotInheritable Class first_const_pair
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function first_compare(Of T1, T2)(ByVal this As first_const_pair(Of T1, T2),
                                                    ByVal that As first_const_pair(Of T1, T2)) As Int32
        Dim c As Int32 = 0
        If object_compare(this, that, c) Then
            Return c
        End If
        assert(this IsNot Nothing)
        assert(that IsNot Nothing)
        Return compare(this.first, that.first)
    End Function

    Public NotInheritable Class first_hasher(Of T1, T2, _HASHER As _to_uint32(Of T1))
        Inherits _to_uint32(Of first_const_pair(Of T1, T2))

        Private Shared ReadOnly hasher As _HASHER = alloc(Of _HASHER)()

        Public Overrides Function at(ByRef k As first_const_pair(Of T1, T2)) As UInt32
            assert(k IsNot Nothing)
            Return hasher(k.first)
        End Function

        Public Overrides Function reverse(ByVal i As UInt32) As first_const_pair(Of T1, T2)
            assert(False)
            Return Nothing
        End Function
    End Class

    Public NotInheritable Class first_equaler(Of T1, T2, _EQUALER As _equaler(Of T1))
        Inherits _equaler(Of first_const_pair(Of T1, T2))

        Private Shared ReadOnly equaler As _EQUALER = alloc(Of _EQUALER)()

        Public Overrides Function at(ByRef i As first_const_pair(Of T1, T2),
                                     ByRef j As first_const_pair(Of T1, T2)) As Boolean
            Dim c As Int32
            If object_compare(i, j, c) Then
                Return c = 0
            End If
            assert(i IsNot Nothing)
            assert(j IsNot Nothing)
            Return equaler(i.first, j.first)
        End Function
    End Class

    Public NotInheritable Class first_comparer(Of T1, T2, _COMPARER As _comparer(Of T1))
        Inherits _comparer(Of first_const_pair(Of T1, T2))

        Private Shared ReadOnly comparer As _COMPARER = alloc(Of _COMPARER)()

        Public Overrides Function at(ByRef i As first_const_pair(Of T1, T2),
                                     ByRef j As first_const_pair(Of T1, T2)) As Int32
            Dim c As Int32
            If object_compare(i, j, c) Then
                Return c
            End If
            assert(i IsNot Nothing)
            assert(j IsNot Nothing)
            Return comparer(i.first, j.first)
        End Function
    End Class
End Class
