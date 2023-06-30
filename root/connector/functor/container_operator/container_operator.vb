
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A functor to implement operators of CONTAINER, the container of T.
Partial Public NotInheritable Class container_operator(Of CONTAINER, T)
    Public Shared ReadOnly r As container_operator(Of CONTAINER, T) = New container_operator(Of CONTAINER, T)()

    Public Shared Sub emplace(ByVal f As Func(Of CONTAINER, T, Boolean))
        global_resolver(Of Func(Of CONTAINER, T, Boolean), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Shared Sub enumerate(ByVal f As Func(Of CONTAINER, container_operator(Of T).enumerator))
        global_resolver(Of Func(Of CONTAINER, container_operator(Of T).enumerator),
                           container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Shared Sub size(ByVal f As Func(Of CONTAINER, UInt32))
        global_resolver(Of Func(Of CONTAINER, UInt32), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Shared Sub clear(ByVal f As Action(Of CONTAINER))
        global_resolver(Of Action(Of CONTAINER), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Function emplace(ByVal i As CONTAINER, ByVal j As T) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim f As Func(Of CONTAINER, T, Boolean) = Nothing
        f = global_resolver(Of Func(Of CONTAINER, T, Boolean), container_operator(Of CONTAINER, T)).resolve_or_null()
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    Public Function insert(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return Not i Is Nothing AndAlso emplace(i, copy_no_error(j))
    End Function

    Public Function enumerate(ByVal i As CONTAINER) As container_operator(Of T).enumerator
        If i Is Nothing Then
            Return Nothing
        End If

        Return global_resolver(Of Func(Of CONTAINER, container_operator(Of T).enumerator),
                                  container_operator(Of CONTAINER, T)).resolve()(i)
    End Function

    Public Function size(ByVal i As CONTAINER) As UInt32
        If i Is Nothing Then
            Return 0
        End If

        Dim f As Func(Of CONTAINER, UInt32) = Nothing
        f = global_resolver(Of Func(Of CONTAINER, UInt32), container_operator(Of CONTAINER, T)).resolve_or_null()
        If Not f Is Nothing Then
            Return f(i)
        End If

        Dim it As container_operator(Of T).enumerator = Nothing
        it = enumerate(i)
        Dim r As UInt32 = 0
        While Not it Is Nothing AndAlso Not it.end()
            r += uint32_1
            it.next()
        End While

        Return r
    End Function

    Public Function empty(ByVal i As CONTAINER) As Boolean
        Return size(i) = uint32_0
    End Function

    Public Sub clear(ByVal i As CONTAINER)
        If i Is Nothing Then
            Return
        End If

        Dim f As Action(Of CONTAINER) = Nothing
        f = global_resolver(Of Action(Of CONTAINER), container_operator(Of CONTAINER, T)).resolve_or_null()
        assert(Not f Is Nothing)
        f(i)
    End Sub

    Private Sub New()
    End Sub
End Class
