﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Structure tuple(Of T1, T2)
    Public Function first() As T1
        Return _1()
    End Function

    Public Function second() As T2
        Return _2()
    End Function

    Public Function to_pair() As pair(Of T1, T2)
        Return pair.of(_1(), _2())
    End Function

    Public Function to_const_pair() As const_pair(Of T1, T2)
        Return const_pair.of(_1(), _2())
    End Function

    Public Function to_first_const_pair() As first_const_pair(Of T1, T2)
        Return first_const_pair.of(_1(), _2())
    End Function

    Public Shared Function from_pair(ByVal p As pair(Of T1, T2)) As tuple(Of T1, T2)
        assert(Not p Is Nothing)
        Return New tuple(Of T1, T2)(p.first, p.second)
    End Function

    Public Shared Function from_const_pair(ByVal p As const_pair(Of T1, T2)) As tuple(Of T1, T2)
        assert(Not p Is Nothing)
        Return New tuple(Of T1, T2)(p.first, p.second)
    End Function

    Public Shared Function from_first_const_pair(ByVal p As first_const_pair(Of T1, T2)) As tuple(Of T1, T2)
        assert(Not p Is Nothing)
        Return New tuple(Of T1, T2)(p.first, p.second)
    End Function

    Public Shared ReadOnly first_selector As Func(Of tuple(Of T1, T2), T1) =
         Function(ByVal i As tuple(Of T1, T2)) As T1
             Return i.first()
         End Function

    Public Shared ReadOnly second_selector As Func(Of tuple(Of T1, T2), T2) =
         Function(ByVal i As tuple(Of T1, T2)) As T2
             Return i.second()
         End Function

    Public Shared ReadOnly first_comparer As Func(Of tuple(Of T1, T2), tuple(Of T1, T2), Int32) =
         Function(ByVal l As tuple(Of T1, T2), ByVal r As tuple(Of T1, T2)) As Int32
             Return compare(l.first(), r.first())
         End Function

    Public Shared ReadOnly second_comparer As Func(Of tuple(Of T1, T2), tuple(Of T1, T2), Int32) =
         Function(ByVal l As tuple(Of T1, T2), ByVal r As tuple(Of T1, T2)) As Int32
             Return compare(l.second(), r.second())
         End Function

    Public Shared ReadOnly second_first_comparer As Func(Of tuple(Of T1, T2), tuple(Of T1, T2), Int32) =
        Function(ByVal l As tuple(Of T1, T2), ByVal r As tuple(Of T1, T2)) As Int32
            Dim cmp As Int32 = 0
            cmp = compare(l.second(), r.second())
            If cmp <> 0 Then
                Return cmp
            End If
            Return compare(l.first(), r.first())
        End Function
End Structure
