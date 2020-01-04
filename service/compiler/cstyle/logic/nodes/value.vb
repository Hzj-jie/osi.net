
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            If b.of(n.child()).build(o) Then
                Return True
            End If
            o.err("@value ", n.child())
            Return False
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer, ByVal parent_node As String) As Boolean
            If build(n, o) Then
                Return True
            End If
            o.err("@", parent_node, " value ", n)
            Return False
        End Function
    End Class
End Class
