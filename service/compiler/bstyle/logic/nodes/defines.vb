
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class ifndef_wrapped
        Inherits ifndef_wrapped(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            MyBase.New(b, Function(ByVal s As String) As Boolean
                              Return scope.current().defines().is_defined(s)
                          End Function)
        End Sub
    End Class

    Private NotInheritable Class define
        Inherits define(Of logic_writer)

        Public Shared ReadOnly instance As New define()

        Private Sub New()
            MyBase.New(Sub(ByVal s As String)
                           scope.current().defines().define(s)
                       End Sub)
        End Sub
    End Class
End Class
