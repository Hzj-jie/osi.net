
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _transformation
    <Extension()> Public Function reverse(ByVal d As Func(Of Boolean)) As Func(Of Boolean)
        If d Is Nothing Then
            Return Function() True
        End If
        Return Function() Not d()
    End Function

    <Extension()> Public Function reverse(Of T)(ByVal d As _do(Of T, Boolean)) As _do(Of T, Boolean)
        If d Is Nothing Then
            Return Function(ByRef x) True
        End If
        Return Function(ByRef x As T) Not d(x)
    End Function

    Private Function v_d_b(ByVal d As Action, ByVal exp As Boolean) As Func(Of Boolean)
        If d Is Nothing Then
            Return Function() As Boolean
                       Return Not exp
                   End Function
        End If
        Return Function() As Boolean
                   d()
                   Return exp
               End Function
    End Function

    Private Function v_d_b(Of T)(ByVal d As Action(Of T), ByVal exp As Boolean) As Func(Of T, Boolean)
        If d Is Nothing Then
            Return Function(ByVal i As T) As Boolean
                       Return Not exp
                   End Function
        End If
        Return Function(ByVal i As T) As Boolean
                   d(i)
                   Return exp
               End Function
    End Function

    <Extension()> Public Function true_(ByVal d As Action) As Func(Of Boolean)
        Return v_d_b(d, True)
    End Function

    <Extension()> Public Function true_(Of T)(ByVal d As Action(Of T)) As Func(Of T, Boolean)
        Return v_d_b(d, True)
    End Function

    <Extension()> Public Function false_(ByVal d As Action) As Func(Of Boolean)
        Return v_d_b(d, False)
    End Function

    <Extension()> Public Function false_(Of T)(ByVal d As Action(Of T)) As Func(Of T, Boolean)
        Return v_d_b(d, False)
    End Function

    <Extension()> Public Function null_or_true(ByVal d As Func(Of Boolean)) As Boolean
        Return d Is Nothing OrElse d()
    End Function

    <Extension()> Public Function null_or_false(ByVal d As Func(Of Boolean)) As Boolean
        Return d Is Nothing OrElse (Not d())
    End Function

    <Extension()> Public Function not_null_and_true(ByVal d As Func(Of Boolean)) As Boolean
        Return Not d Is Nothing AndAlso d()
    End Function

    <Extension()> Public Function not_null_and_false(ByVal d As Func(Of Boolean)) As Boolean
        Return Not d Is Nothing AndAlso (Not d())
    End Function

    <Extension()> Public Function type_erasure(Of T)(ByVal v As Action(Of Object)) As Action(Of T)
        If v Is Nothing Then
            Return Nothing
        End If
        Return Sub(ByVal x As T)
                   v(x)
               End Sub
    End Function

    <Extension()> Public Function parameter_erasure(Of T)(ByVal i As Action) As Action(Of T)
        If i Is Nothing Then
            Return Nothing
        End If
        Return Sub(ByVal v As T)
                   i()
               End Sub
    End Function

    <Extension()> Public Function parameter_erasure(Of T, RT)(ByVal i As Func(Of RT)) As Func(Of T, RT)
        If i Is Nothing Then
            Return Nothing
        End If
        Return Function(ByVal v As T) As RT
                   Return i()
               End Function
    End Function
End Module
