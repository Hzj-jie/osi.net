
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class type_equaler
    Private Shared ReadOnly ss As type_resolver(Of Func(Of Object, Object, Boolean)) =
        type_resolver(Of Func(Of Object, Object, Boolean)).default

    Public Shared Function defined(ByVal i As Type, ByVal j As Type) As Boolean
        Return ss.registered(typed(i, j))
    End Function

    Public Shared Function ref(ByVal i As Type, ByVal j As Type) As Func(Of Object, Object, Boolean)
        Dim f As Func(Of Object, Object, Boolean) = Nothing
        If Not ss.from_type(typed(i, j), f) Then
            Return Nothing
        End If
        Return f
    End Function

    Public Shared Function equal(ByVal i As Type,
                                 ByVal j As Type,
                                 ByVal x As Object,
                                 ByVal y As Object,
                                 ByRef o As Boolean) As Boolean
        Dim f As Func(Of Object, Object, Boolean) = Nothing
        If Not ss.from_type(typed(i, j), f) Then
            Return False
        End If
        o = f(x, y)
        Return True
    End Function

    Public Shared Function equal(ByVal i As Type, ByVal j As Type, ByVal x As Object, ByVal y As Object) As Boolean
        Dim o As Boolean = False
        If equal(i, j, x, y, o) Then
            Return o
        End If
        Return runtime_equal(x, y)
    End Function

    Public Shared Function infer_equal(ByVal x As Object, ByVal y As Object) As Boolean
        assert(Not x Is Nothing)
        assert(Not y Is Nothing)
        Return equal(x.GetType(), y.GetType(), x, y)
    End Function

    Private Shared Function typed(ByVal i As Type, ByVal j As Type) As Type
        Return joint_type.of(i, j, GetType(equaler))
    End Function

    Private Sub New()
    End Sub
End Class
