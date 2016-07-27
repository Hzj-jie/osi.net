
Imports System.Runtime.CompilerServices

Public Module _transformation
    Public Function reverse(ByVal d As Func(Of Boolean)) As Func(Of Boolean)
        If d Is Nothing Then
            Return Function() True
        Else
            Return Function() Not d()
        End If
    End Function

    Public Function reverse(Of T)(ByVal d As _do(Of T, Boolean)) As _do(Of T, Boolean)
        If d Is Nothing Then
            Return Function(ByRef x) True
        Else
            Return Function(ByRef x As T) Not d(x)
        End If
    End Function

    Private Function v_d_b(ByVal d As Action, ByVal exp As Boolean) As Func(Of Boolean)
        If d Is Nothing Then
            Return Function() Not exp
        Else
            Return Function() As Boolean
                       d()
                       Return exp
                   End Function
        End If
    End Function

    Public Function true_(ByVal d As Action) As Func(Of Boolean)
        Return v_d_b(d, True)
    End Function

    Public Function false_(ByVal d As Action) As Func(Of Boolean)
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
End Module
