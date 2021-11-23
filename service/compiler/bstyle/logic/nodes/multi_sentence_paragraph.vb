
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class multi_sentence_paragraph
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of multi_sentence_paragraph)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return builders.start_scope(o).of(
                       Function() As Boolean
                           Using New scope_wrapper()
                               Dim i As UInt32 = 1
                               While i < n.child_count() - uint32_1
                                   If Not l.of(n.child(i)).build(o) Then
                                       Return False
                                   End If
                                   i += uint32_1
                               End While
                           End Using
                           Return True
                       End Function)
        End Function
    End Class
End Class
