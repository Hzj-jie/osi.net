
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

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

        Public Shared Function of_callee(ByVal ta As type_alias,
                                         ByVal name As String,
                                         ByVal type As String,
                                         ByVal parameters As vector(Of pair(Of String, String)),
                                         ByVal paragraph As Func(Of Boolean)) As callee_builder_12
            assert(Not ta Is Nothing)
            Return of_callee(name,
                             ta(type),
                             parameters.map(Function(ByVal i As pair(Of String, String)) As pair(Of String, String)
                                                assert(Not i Is Nothing)
                                                Return pair.emplace_of(i.first, ta(i.second))
                                            End Function),
                             paragraph)
        End Function

        Private Sub New()
            End Sub
        End Class
End Namespace
