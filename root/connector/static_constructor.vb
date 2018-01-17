
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
        Else
            Return Sub()
                       c.Invoke(Nothing, Nothing)
                   End Sub
        End If
    End Function

    Public Shared Function as_action(ByVal t As Type) As Action
        Return as_action(retrieve(t))
    End Function

    ' This function won't guarantee the TypeInitializer will be executed only once.
    Public Shared Sub execute(ByVal t As Type)
        Dim a As Action = Nothing
        a = as_action(t)
        If Not a Is Nothing Then
            a()
        End If
    End Sub

    Private ReadOnly c As ConstructorInfo
    Private ReadOnly a As Action

    Public Sub New(ByVal t As Type)
        c = retrieve(t)
        a = as_action(c)
    End Sub

    Public Function retrieve() As ConstructorInfo
        Return c
    End Function

    Public Function as_action() As Action
        Return a
    End Function

    Public Sub execute()
        If Not a Is Nothing Then
            a()
        End If
    End Sub
End Class

Public NotInheritable Class static_constructor(Of T)
    Private NotInheritable Class executor
        Shared Sub New()
            Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(GetType(T).TypeHandle())
        End Sub

        Public Shared Sub execute()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class holder
        Public Shared ReadOnly s As static_constructor

        Shared Sub New()
            s = New static_constructor(GetType(T))
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function static_constructor() As static_constructor
        Return holder.s
    End Function

    Public Shared Function retrieve() As ConstructorInfo
        Return static_constructor().retrieve()
    End Function

    Public Shared Function as_action() As Action
        Return static_constructor().as_action()
    End Function

    ' This function guarantees the TypeInitializer will be executed only once, even not through this function.
    Public Shared Sub execute()
        executor.execute()
    End Sub

    Private Sub New()
    End Sub
End Class
