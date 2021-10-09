
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class binary_operator_registry2
    Private Shared Sub init()
        binary_operator.register_add(Function(ByVal x As String, ByVal y As String) As String
                                         Return x + y
                                     End Function)
        binary_operator.register_add(Function(ByVal x As Char, ByVal y As Char) As String
                                         Return x + y
                                     End Function)
    End Sub

    Private Sub New()
    End Sub
End Class
