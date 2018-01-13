
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class caster
    Public Shared Sub register(Of IT, OT)(ByVal f As Func(Of IT, OT))
        caster(Of IT, OT).register(f)
    End Sub

    Private Sub New()
    End Sub
End Class

' Implementations can assume i is not null. Users should not use this class directly except for the registration.
Public NotInheritable Class caster(Of IT, OT)
    Public Shared Sub register(ByVal c As Func(Of IT, OT))
        global_resolver(Of Func(Of IT, OT), caster(Of IT, OT)).assert_first_register(c)
    End Sub

    Public Shared Sub unregister()
        global_resolver(Of Func(Of IT, OT), caster(Of IT, OT)).unregister()
    End Sub

    Public Shared Function defined() As Boolean
        Return global_resolver(Of Func(Of IT, OT), caster(Of IT, OT)).registered()
    End Function

    Public Shared Function ref() As Func(Of IT, OT)
        Return global_resolver(Of Func(Of IT, OT), caster(Of IT, OT)).resolve_or_null()
    End Function

    Public Shared Function cast(ByVal i As IT) As OT
        Return global_resolver(Of Func(Of IT, OT), caster(Of IT, OT)).resolve()(i)
    End Function

    Private Sub New()
    End Sub
End Class