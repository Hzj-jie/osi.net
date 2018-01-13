
Option Explicit On
Option Infer Off
Option Strict On

' TODO

Imports osi.root.connector
Imports osi.service.argument

<AttributeUsage(AttributeTargets.Constructor, AllowMultiple:=False, Inherited:=False)>
Public NotInheritable Class injector_constructor
    Inherits Attribute
End Class

' For an implementation to use predefined injector constructor.
Public NotInheritable Class injector_constructor(Of T)
    Private Shared ReadOnly f As Func(Of Object(), T)

    Shared Sub New()
        f = type_info(Of T).annotated_constructor(Of injector_constructor)()
        assert(Not f Is Nothing)
    End Sub

    Private Sub New()
    End Sub
End Class
