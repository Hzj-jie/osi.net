
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value_list
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() > 0)
            Dim v As New vector(Of String)()
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                Using r As read_scoped(Of scope.value_target_t.target).ref = value.read_target()
                    v.emplace_back((+r).names)
                End Using
                i += uint32_1
            End While
            scope.current().value_target().with_value_list(v)
            Return True
        End Function

        Public Shared Function current_targets() As read_scoped(Of vector(Of String)).ref
            Return scope.current().value_target().value_list()
        End Function
    End Class
End Class
