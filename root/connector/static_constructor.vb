
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

' TODO: Move to type_info.
Public NotInheritable Class static_constructor
    Public Shared Function retrieve(ByVal t As Type) As ConstructorInfo
        ' See
        ' https://stackoverflow.com/questions/11593615/why-would-finding-a-types-initializer-throw-a-nullreferenceexception
        ' about the reason of casting t to _Type.
        ' It may randomly throw System.NullReferenceException within System.RuntimeType.GetConstructorImpl().
        Return If(t Is Nothing, Nothing, direct_cast(Of Runtime.InteropServices._Type)(t).TypeInitializer())
    End Function

    Private Shared Function as_action(ByVal c As ConstructorInfo) As Action
        If c Is Nothing Then
            Return Nothing
        End If
        Return Sub()
                   c.Invoke(Nothing, Nothing)
               End Sub
    End Function

    Public Shared Function as_action(ByVal t As Type) As Action
        Return as_action(retrieve(t))
    End Function

    Public Shared Function as_once_action(ByVal t As Type) As Action
        If t Is Nothing Then
            Return Nothing
        End If

        Return Sub()
                   once_execute(t)
               End Sub
    End Function

    ' This function won't guarantee the TypeInitializer will be executed only once.
    Public Shared Sub execute(ByVal t As Type)
        Dim a As Action = Nothing
        a = as_action(t)
        If a IsNot Nothing Then
            a()
        End If
    End Sub

    Public Shared Sub once_execute(ByVal t As Type)
        If t IsNot Nothing Then
            Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.TypeHandle())
        End If
    End Sub

    Private ReadOnly c As ConstructorInfo
    Private ReadOnly a As Action
    Private ReadOnly oa As Action

    Public Sub New(ByVal t As Type)
        assert(t IsNot Nothing)
        c = retrieve(t)
        a = as_action(c)
        oa = as_once_action(t)
        assert(oa IsNot Nothing)
    End Sub

    Public Function retrieve() As ConstructorInfo
        Return c
    End Function

    Public Function as_action() As Action
        Return a
    End Function

    Public Function as_once_action() As Action
        Return oa
    End Function

    Public Sub execute()
        If a IsNot Nothing Then
            a()
        End If
    End Sub

    Public Sub once_execute()
        oa()
    End Sub
End Class

Public NotInheritable Class static_constructor(Of T)
    Public Shared Function static_constructor() As static_constructor
        Return type_info(Of T).static_constructor()
    End Function

    Public Shared Function retrieve() As ConstructorInfo
        Return static_constructor().retrieve()
    End Function

    Public Shared Function as_action() As Action
        Return static_constructor().as_action()
    End Function

    ' This function guarantees the TypeInitializer will be executed only once, even not through this function.
    Public Shared Sub execute()
        static_constructor().once_execute()
    End Sub

    Private Sub New()
    End Sub
End Class
