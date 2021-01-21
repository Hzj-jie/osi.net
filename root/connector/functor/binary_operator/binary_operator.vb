
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A functor to implement binary operators between T and T2, and return RT.
Public Class binary_operator(Of T, T2, RT)
    Public Shared ReadOnly r As binary_operator(Of T, T2, RT) = New binary_operator(Of T, T2, RT)()

    Public Shared Sub register_add(ByVal f As Func(Of T, T2, RT))
        global_resolver(Of Func(Of T, T2, RT), add_protector).assert_first_register(f)
    End Sub

    Public Shared Sub register_minus(ByVal f As Func(Of T, T2, RT))
        global_resolver(Of Func(Of T, T2, RT), minus_protector).assert_first_register(f)
    End Sub

    Public Shared Sub register_multiply(ByVal f As Func(Of T, T2, RT))
        global_resolver(Of Func(Of T, T2, RT), multiply_protector).assert_first_register(f)
    End Sub

    Public Shared Function log_addable() As Boolean
        If global_resolver(Of Func(Of T, T2, RT), add_protector).registered() OrElse
           accumulatable(Of T, T2, RT).v Then
            Return True
        End If
        assert(Not accumulatable(Of T, T2, RT).ex Is Nothing)
        raise_error(error_type.warning,
                    "cannot add a T (",
                    GetType(T).full_name(),
                    ") to T2 (",
                    GetType(T2).full_name(),
                    ") and return RT (",
                    GetType(RT).full_name(),
                    "), ex ",
                    accumulatable(Of T, T2, RT).ex.details())
        Return False
    End Function

    ' Return the value of i + j
    Public Function add(ByVal i As T, ByVal j As T2) As RT
        Dim f As Func(Of T, T2, RT) = Nothing
        f = global_resolver(Of Func(Of T, T2, RT), add_protector).resolve_or_null()
        If Not f Is Nothing Then
            Return f(i, j)
        End If
        assert(accumulatable(Of T, T2, RT).v)
        Return cast(Of RT)(implicit_conversions.object_add(i, j))
    End Function

    ' Return the value of i - j
    Public Function minus(ByVal i As T, ByVal j As T2) As RT
        Dim f As Func(Of T, T2, RT) = Nothing
        f = global_resolver(Of Func(Of T, T2, RT), minus_protector).resolve_or_null()
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    ' Return the value of i - j
    Public Function multiply(ByVal i As T, ByVal j As T2) As RT
        Dim f As Func(Of T, T2, RT) = Nothing
        f = global_resolver(Of Func(Of T, T2, RT), multiply_protector).resolve_or_null()
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    Public Shared Operator +(ByVal this As binary_operator(Of T, T2, RT)) As binary_operator(Of T, T2, RT)
        If this Is Nothing Then
            Return r
        End If
        Return this
    End Operator

    Private Interface add_protector
    End Interface

    Private Interface minus_protector
    End Interface

    Private Interface multiply_protector
    End Interface

    Protected Sub New()
    End Sub
End Class
