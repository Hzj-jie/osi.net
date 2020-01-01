
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class self_dec
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of self_dec)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            builders.of_subtract(value.current_target(), value.current_target(), constants.int_1).to(o)
            Return True
        End Function
    End Class
End Class
