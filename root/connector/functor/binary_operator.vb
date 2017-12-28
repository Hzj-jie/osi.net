
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Class binary_operator(Of T, T2, RT)
    Public Shared ReadOnly [default] As binary_operator(Of T, T2, RT)

    Shared Sub New()
        [default] = New binary_operator(Of T, T2, RT)()
    End Sub

    Public Shared Sub register_add(ByVal f As Func(Of T, T2, RT))
        binder(Of Func(Of T, T2, RT), binary_operator_add_protector).set_global(f)
    End Sub

    Public Shared Sub register_minus(ByVal f As Func(Of T, T2, RT))
        binder(Of Func(Of T, T2, RT), binary_operator_minus_protector).set_global(f)
    End Sub

    Public Overridable Function add(ByVal i As T, ByVal j As T2) As RT
        assert(binder(Of Func(Of T, T2, RT), binary_operator_add_protector).has_global())
        Return binder(Of Func(Of T, T2, RT), binary_operator_add_protector).global()(i, j)
    End Function

    Public Overridable Function minus(ByVal i As T, ByVal j As T2) As RT
        assert(binder(Of Func(Of T, T2, RT), binary_operator_minus_protector).has_global())
        Return binder(Of Func(Of T, T2, RT), binary_operator_minus_protector).global()(i, j)
    End Function

    Public Shared Operator +(ByVal this As binary_operator(Of T, T2, RT)) As binary_operator(Of T, T2, RT)
        If this Is Nothing Then
            Return [default]
        Else
            Return this
        End If
    End Operator

    Protected Sub New()
    End Sub
End Class

Public Class binary_operator(Of T)
    Inherits binary_operator(Of T, T, T)

    Protected Sub New()
    End Sub
End Class
