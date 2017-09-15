﻿
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class comparer
    Public Shared Sub register(Of T, T2)(ByVal c As Func(Of T, T2, Int32))
        assert(Not c Is Nothing)
        comparer(Of T, T2).register(c)
        If Not type_info(Of T, type_info_operators.is, T2).v Then
            comparer(Of T2, T).register(Function(i As T2, j As T) As Int32
                                            Return -c(j, i)
                                        End Function)
        End If
    End Sub

    Public Shared Function compare(Of T, T2)(ByVal i As T, ByVal j As T2) As Int32
        Return comparer(Of T, T2).compare(i, j)
    End Function

    Private Sub New()
    End Sub
End Class

' Implementation can assume i and j are not null. Users should not use this class directly, except for register.
Public NotInheritable Class comparer(Of T, T2)
    Private Shared c As Func(Of T, T2, Int32)

    Public Shared Sub register(ByVal c As Func(Of T, T2, Int32))
        assert(Not c Is Nothing)
        assert(comparer(Of T, T2).c Is Nothing OrElse object_compare(comparer(Of T, T2).c, c) = 0)
        comparer(Of T, T2).c = c
    End Sub

    Public Shared Sub unregister()
        assert(Not c Is Nothing)
        c = Nothing
    End Sub

    Public Shared Function defined() As Boolean
        Return Not c Is Nothing
    End Function

    Public Shared Function ref() As Func(Of T, T2, Int32)
        Return c
    End Function

    Public Shared Function compare(ByVal i As T, ByVal j As T2) As Int32
        assert(Not c Is Nothing)
        Return c(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
