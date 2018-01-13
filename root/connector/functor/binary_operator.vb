
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A functor to implement binary operators between T and T2, and return RT.
Public Class binary_operator(Of T, T2, RT)
    Public Shared ReadOnly [default] As binary_operator(Of T, T2, RT)

    Shared Sub New()
        [default] = New binary_operator(Of T, T2, RT)()
    End Sub

    Public Shared Sub register_add(ByVal f As Func(Of T, T2, RT))
        global_resolver(Of Func(Of T, T2, RT), add_protector).assert_first_register(f)
    End Sub

    Public Shared Sub register_minus(ByVal f As Func(Of T, T2, RT))
        global_resolver(Of Func(Of T, T2, RT), minus_protector).assert_first_register(f)
    End Sub

    Public Shared Function log_addable() As Boolean
        If Not global_resolver(Of Func(Of T, T2, RT), add_protector).registered() AndAlso
           Not accumulatable(Of T, T2, RT).v Then
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
        Else
            Return True
        End If
    End Function

    Protected Overridable Function add() As Func(Of T, T2, RT)
        Return global_resolver(Of Func(Of T, T2, RT), add_protector).resolve_or_null()
    End Function

    ' Return the value of i + j
    Public Function add(ByVal i As T, ByVal j As T2) As RT
        Dim f As Func(Of T, T2, RT) = Nothing
        f = add()
        If Not f Is Nothing Then
            Return f(i, j)
        Else
            assert(accumulatable(Of T, T2, RT).v)
            Return cast(Of RT)(implicit_conversions.object_add(i, j))
        End If
    End Function

    Protected Overridable Function minus() As Func(Of T, T2, RT)
        Return global_resolver(Of Func(Of T, T2, RT), minus_protector).resolve_or_null()
    End Function

    ' Return the value of i - j
    Public Overridable Function minus(ByVal i As T, ByVal j As T2) As RT
        Dim f As Func(Of T, T2, RT) = Nothing
        f = minus()
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    Public Shared Operator +(ByVal this As binary_operator(Of T, T2, RT)) As binary_operator(Of T, T2, RT)
        If this Is Nothing Then
            Return [default]
        Else
            Return this
        End If
    End Operator

    Private Interface add_protector
    End Interface

    Private Interface minus_protector
    End Interface

    Protected Sub New()
    End Sub
End Class
