
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.envs

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public Shared ReadOnly debug_dump As Boolean

    Shared Sub New()
        debug_dump = env_bool(env_keys("rewrite", "debug", "dump"))
    End Sub

    Public Interface statement
        Inherits statement(Of typed_node_writer)
    End Interface

    Public NotInheritable Class statements
        Inherits statements(Of typed_node_writer)
    End Class
End Class
