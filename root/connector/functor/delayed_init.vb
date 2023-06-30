
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class delayed_init(Of PROTECTOR)
    Public Shared Sub register(ByVal f As Action)
        global_resolver(Of Action, delayed_init(Of PROTECTOR)).assert_first_register(f)
    End Sub

    Private NotInheritable Class once
        Shared Sub New()
            Dim f As Action = global_resolver(Of Action, delayed_init(Of PROTECTOR)).resolve_or_null()
            If Not f Is Nothing Then
                f()
            End If
        End Sub

        Public Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Sub execute()
        once.init()
    End Sub

    Private Sub New()
    End Sub
End Class
