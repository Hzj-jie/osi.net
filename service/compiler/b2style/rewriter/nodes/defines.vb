
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class ifndef_wrapped
        Inherits ifndef_wrapped(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b, Function(ByVal s As String) As Boolean
                              Return scope.current().defines().is_defined(s)
                          End Function)
        End Sub
    End Class

    Public NotInheritable Class define
        Inherits define(Of typed_node_writer)

        Public Shared ReadOnly instance As New define()

        Private Sub New()
            MyBase.New(Sub(ByVal s As String)
                           scope.current().defines().define(s)
                       End Sub)
        End Sub
    End Class
End Class
