
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class name
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Shared Function [of](ByVal name As String) As Action(Of code_gens(Of typed_node_writer))
            Return Sub(ByVal b As code_gens(Of typed_node_writer))
                       assert(Not b Is Nothing)
                       b.register(Of name)(name)
                   End Sub
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            o.append(namespace_.bstyle_format(n.children_word_str()))
            Return True
        End Function
    End Class
End Class
