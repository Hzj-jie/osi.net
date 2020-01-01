
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class multi_sentence_paragraph
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of multi_sentence_paragraph)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 2)
            Dim i As UInt32 = 1
            While i < n.child_count() - 1
                If Not b.of(n.child(i)).build(o) Then
                    o.err("@multi_sentence_paragraph sentence ", i, " - ", n.child(i))
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function
    End Class
End Class
