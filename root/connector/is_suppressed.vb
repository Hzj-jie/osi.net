
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class is_suppressed
    Public Shared Function by(ByVal key As String) As Boolean
        Return do_(global_resolver(Of Func(Of String, Boolean), is_suppressed).resolve_or_null(), key, False)
    End Function

    Public Shared Function by(Of PROTECTOR)() As Boolean
        Return do_(global_resolver(Of Func(Of Boolean), PROTECTOR).resolve_or_null(), False)
    End Function

    Public Shared Function alloc_error() As Boolean
        Return by(Of alloc_error_protector)()
    End Function

    Public Shared Function compare_error() As Boolean
        Return by(Of compare_error_protector)()
    End Function

    Public Shared Function rebind_global_value() As Boolean
        Return by(Of rebind_global_value_protector)()
    End Function

    Public Interface alloc_error_protector
    End Interface

    Public Interface compare_error_protector
    End Interface

    Public Interface rebind_global_value_protector
    End Interface

    Private Sub New()
    End Sub
End Class
