
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class is_suppressed
    Public Shared Function true_(Of PROTECTOR)() As Boolean
        Return do_(global_resolver(Of Func(Of Boolean), PROTECTOR).resolve_or_null(), False)
    End Function

    Public Shared Function alloc_error() As Boolean
        Return true_(Of alloc_error_protector)()
    End Function

    Public Shared Function compare_error() As Boolean
        Return true_(Of compare_error_protector)()
    End Function

    Public Shared Function rebind_global_value() As Boolean
        Return true_(Of rebind_global_value_protector)()
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
