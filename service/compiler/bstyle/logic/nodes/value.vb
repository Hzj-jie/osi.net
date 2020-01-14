
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly ta As type_alias

        Public Sub New(ByVal b As logic_gens, ByVal ta As type_alias)
            MyBase.New(b)
            Me.ta = ta
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal l As logic_rule_wrapper)
            assert(Not b Is Nothing)
            assert(Not l Is Nothing)
            b.register(New value(b, l.type_alias))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(strsame(n.type_name, "value") OrElse
                   (strsame(n.type_name, "ignore-result-function-call") AndAlso
                    strsame(n.child().type_name, "function-call")))
            with_temp_target(ta, n, o)
            Return l.of(n.child()).build(o)
        End Function
    End Class
End Class
