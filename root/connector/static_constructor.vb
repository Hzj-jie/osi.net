
Imports System.Reflection

Public Class static_constructor(Of T)
    Private Class executor
        Shared Sub New()
            Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(GetType(T).TypeHandle())
        End Sub

        Public Shared Sub execute()
        End Sub
    End Class

    Public Shared Function retrieve() As ConstructorInfo
        Return GetType(T).TypeInitializer()
    End Function

    Public Shared Function as_action() As Action
        Dim c As ConstructorInfo = Nothing
        c = retrieve()
        If c Is Nothing Then
            Return Nothing
        Else
            Return Sub()
                       c.Invoke(Nothing, Nothing)
                   End Sub
        End If
    End Function

    Public Shared Sub execute()
        executor.execute()
    End Sub
End Class
