
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

Namespace logic
    Partial Public NotInheritable Class builders
        Public Shared ReadOnly debug_dump As Boolean

        Shared Sub New()
            debug_dump = env_bool(env_keys("compiler", "debug", "dump"))
        End Sub

        Public Shared Function of_define(ByVal ta As type_alias,
                                         ByVal name As String,
                                         ByVal type As String) As define_builder_24
            assert(Not ta Is Nothing)
            Return of_define(name, ta(type))
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
