
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class ifndef_wrapped
        Inherits ifndef_wrapped(Of typed_node_writer)

        Public Sub New()
            MyBase.New(AddressOf code_gen_of,
                       Function(ByVal s As String) As Boolean
                           Return scope.current().defines().is_defined(s)
                       End Function)
        End Sub
    End Class

    Private NotInheritable Class define
        Inherits define(Of typed_node_writer)

        Public Sub New()
            MyBase.New(Sub(ByVal s As String)
                           scope.current().defines().define(s)
                       End Sub)
        End Sub
    End Class
End Class
