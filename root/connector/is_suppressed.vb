
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class is_suppressed
    Private NotInheritable Class suppressed(Of PROTECTOR)
        Private Shared ReadOnly binder As binder(Of Func(Of Boolean), PROTECTOR)

        Public Shared Function true_() As Boolean
            Return binder.has_value() AndAlso (+binder)()
        End Function

        Private Sub New()
        End Sub
    End Class

    Public Shared Function alloc_error() As Boolean
        Return suppressed(Of suppress_alloc_error_binder_protector).true_()
    End Function

    Public Shared Function compare_error() As Boolean
        Return suppressed(Of suppress_compare_error_binder_protector).true_()
    End Function

    Public Shared Function rebind_global_value() As Boolean
        Return suppressed(Of suppress_rebind_global_value_error_binder_protector).true_()
    End Function

    Private Sub New()
    End Sub
End Class
