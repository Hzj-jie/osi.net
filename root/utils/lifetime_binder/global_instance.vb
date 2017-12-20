
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public NotInheritable Class global_instance(Of T, _new As _new(Of T))
    Private NotInheritable Class initialized
        Public Shared v As Boolean

        Shared Sub New()
            assert(Not v)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class instance
        Public Shared ReadOnly v As T

        Shared Sub New()
            v = +alloc(Of _new)()
            application_lifetime.stopping_handle(Sub()
                                                     disposable.dispose(v)
                                                 End Sub)
            initialized.v = True
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function ref_new() As T
        Return instance.v
    End Function

    Public Shared Function ref() As T
        If initialized.v Then
            Return instance.v
        Else
            Return Nothing
        End If
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class global_instance(Of T)
    Public Shared Function ref_new() As T
        Return global_instance(Of T, default_new(Of T)).ref_new()
    End Function

    Public Shared Function ref() As T
        Return global_instance(Of T, default_new(Of T)).ref()
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class newable_global_instance(Of T As New)
    Public Shared Function ref_new() As T
        Return global_instance(Of T, newable_new(Of T)).ref_new()
    End Function

    Public Shared Function ref() As T
        Return global_instance(Of T, newable_new(Of T)).ref()
    End Function

    Private Sub New()
    End Sub
End Class