
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

' A functor to implement operators of CONTAINER, the container of T.
Partial Public Class container_operator(Of CONTAINER, T)
    Public Shared ReadOnly [default] As container_operator(Of CONTAINER, T)

    Shared Sub New()
        [default] = New container_operator(Of CONTAINER, T)()
    End Sub

    Public Shared Sub register(ByVal f As Func(Of CONTAINER, T, Boolean))
        global_resolver(Of Func(Of CONTAINER, T, Boolean), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Shared Sub register(ByVal f As Func(Of CONTAINER, enumerator))
        global_resolver(Of Func(Of CONTAINER, enumerator), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Public Shared Sub register(ByVal f As Func(Of CONTAINER, UInt32))
        global_resolver(Of Func(Of CONTAINER, UInt32), container_operator(Of CONTAINER, T)).assert_first_register(f)
    End Sub

    Protected Overridable Function emplace() As Func(Of CONTAINER, T, Boolean)
        Return global_resolver(Of Func(Of CONTAINER, T, Boolean), container_operator(Of CONTAINER, T)).resolve_or_null()
    End Function

    Public Function emplace(ByVal i As CONTAINER, ByVal j As T) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim f As Func(Of CONTAINER, T, Boolean) = Nothing
        f = emplace()
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    Public Function insert(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return Not i Is Nothing AndAlso emplace(i, copy_no_error(j))
    End Function

    Protected Overridable Function enumerate() As Func(Of CONTAINER, enumerator)
        Return global_resolver(Of Func(Of CONTAINER, enumerator), container_operator(Of CONTAINER, T)).resolve_or_null()
    End Function

    Public Function enumerate(ByVal i As CONTAINER) As enumerator
        If i Is Nothing Then
            Return Nothing
        End If

        Dim f As Func(Of CONTAINER, enumerator) = Nothing
        f = enumerate()
        assert(Not f Is Nothing)
        Return f(i)
    End Function

    Protected Overridable Function size() As Func(Of CONTAINER, UInt32)
        Return global_resolver(Of Func(Of CONTAINER, UInt32), container_operator(Of CONTAINER, T)).resolve_or_null()
    End Function

    Public Function size(ByVal i As CONTAINER) As UInt32
        If i Is Nothing Then
            Return 0
        End If

        Dim f As Func(Of CONTAINER, UInt32) = Nothing
        f = size()
        If Not f Is Nothing Then
            Return f(i)
        End If

        Dim it As enumerator = Nothing
        it = enumerate(i)
        Dim r As UInt32 = 0
        While Not it Is Nothing AndAlso Not it.end()
            r += uint32_1
            it.next()
        End While

        Return r
    End Function

    Protected Sub New()
    End Sub
End Class
