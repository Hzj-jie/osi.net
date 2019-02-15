
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class comparer
    Public Shared Sub register(Of T, T2)(ByVal c As Func(Of T, T2, Int32))
        comparer(Of T, T2).register(c)
    End Sub

    Public Shared Function compare(Of T, T2)(ByVal i As T, ByVal j As T2) As Int32
        Return comparer(Of T, T2).compare(i, j)
    End Function

    Private Sub New()
    End Sub
End Class

' Implementations can assume i and j are not null. Users should not use this class directly except for the registration.
Public NotInheritable Class comparer(Of T, T2)
    Public Shared Sub register(ByVal c As Func(Of T, T2, Int32))
        assert(Not c Is Nothing)
        register(Of T, T2)(c)
        If Not type_info(Of T, type_info_operators.is, T2).v Then
            register(Of T2, T)(Function(i As T2, j As T) As Int32
                                   Return -c(j, i)
                               End Function)
        End If
    End Sub

    Private Shared Sub register(Of IT, IT2)(ByVal c As Func(Of IT, IT2, Int32))
        global_resolver(Of Func(Of IT, IT2, Int32), comparer(Of IT, IT2)).assert_first_register(c)
        type_resolver(Of Func(Of Object, Object, Int32)).default.assert_first_register(
            GetType(joint_type(Of IT, IT2, comparer)),
            comparer_object(c))
    End Sub

    Public Shared Sub unregister()
        unregister(Of T, T2)()
        If Not type_info(Of T, type_info_operators.is, T2).v Then
            unregister(Of T2, T)()
        End If
    End Sub

    Private Shared Sub unregister(Of IT, IT2)()
        global_resolver(Of Func(Of IT, IT2, Int32), comparer(Of IT, IT2)).assert_unregister()
        type_resolver(Of Func(Of Object, Object, Int32)).default.
            assert_unregister(GetType(joint_type(Of IT, IT2, comparer)))
    End Sub

    Public Shared Function defined() As Boolean
        Return global_resolver(Of Func(Of T, T2, Int32), comparer(Of T, T2)).registered()
    End Function

    Public Shared Function ref() As Func(Of T, T2, Int32)
        Return global_resolver(Of Func(Of T, T2, Int32), comparer(Of T, T2)).resolve_or_null()
    End Function

    Public Shared Function compare(ByVal i As T, ByVal j As T2) As Int32
        Return global_resolver(Of Func(Of T, T2, Int32), comparer(Of T, T2)).resolve()(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
