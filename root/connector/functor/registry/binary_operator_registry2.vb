
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class binary_operator_registry2
    Shared Sub New()
        binary_operator.register_add(Function(x As String, y As String) As String
                                         Return x + y
                                     End Function)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
