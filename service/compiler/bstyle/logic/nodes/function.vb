
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [function]
        Inherits logic_gen_wrapper
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of [function])()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim has_paramlist As Boolean = strsame(n.child(3).type_name, "paramlist")
            If has_paramlist Then
                If Not l.of(n.child(3)).build(o) Then
                    Return False
                End If
            Else
                logic_gen_of(Of paramlist)().empty_paramlist()
            End If
            Using params As read_scoped(Of vector(Of builders.parameter)).ref =
                    l.typed_code_gen(Of paramlist)().current_target()
                Using New scope_wrapper()
                    Return logic_name.of_callee(n.child(1).word().str(),
                                                n.child(0).word().str(),
                                                +params,
                                                Function() As Boolean
                                                    Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                    Return l.of(n.child(gi)).build(o)
                                                End Function,
                                                o)
                End Using
            End Using
        End Function
    End Class
End Class
