
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class type_comparer
    Private Shared ReadOnly ss As type_resolver(Of Func(Of Object, Object, Int32))

    Shared Sub New()
        ss = type_resolver(Of Func(Of Object, Object, Int32)).default
    End Sub

    Public Shared Function defined(ByVal i As Type, ByVal j As Type) As Boolean
        Return ss.registered(typed(i, j))
    End Function

    Public Shared Function ref(ByVal i As Type, ByVal j As Type) As Func(Of Object, Object, Int32)
        Dim f As Func(Of Object, Object, Int32) = Nothing
        If Not ss.from_type(typed(i, j), f) Then
            Return Nothing
        End If
        Return f
    End Function

    Public Shared Function compare(ByVal i As Type,
                                   ByVal j As Type,
                                   ByVal x As Object,
                                   ByVal y As Object,
                                   ByRef o As Int32) As Boolean
        Dim f As Func(Of Object, Object, Int32) = Nothing
        If Not ss.from_type(typed(i, j), f) Then
            Return False
        End If
        o = f(x, y)
        Return True
    End Function

    Public Shared Function compare(ByVal i As Type, ByVal j As Type, ByVal x As Object, ByVal y As Object) As Int32
        Dim o As Int32 = 0
        If compare(i, j, x, y, o) Then
            Return o
        End If
        Return runtime_compare(x, y)
    End Function

    Public Shared Function infer_compare(ByVal x As Object, ByVal y As Object) As Int32
        assert(Not x Is Nothing)
        assert(Not y Is Nothing)
        Return compare(x.GetType(), y.GetType(), x, y)
    End Function

    Private Shared Function typed(ByVal i As Type, ByVal j As Type) As Type
        Return joint_type.of(i, j, GetType(comparer))
    End Function

    Private Sub New()
    End Sub
End Class
