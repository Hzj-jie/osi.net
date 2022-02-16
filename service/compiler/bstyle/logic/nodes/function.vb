
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class _function
        Implements code_gen(Of logic_writer)

        Private Shared remove_unused_functions As argument(Of Boolean)
        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using new_scope As scope = scope.current().start_scope()
                Dim fo As New logic_writer()
                Dim has_paramlist As Boolean = strsame(n.child(3).type_name, "paramlist")
                If has_paramlist Then
                    If Not l.of(n.child(3)).build(o) Then
                        Return False
                    End If
                End If
                Dim function_name As String = n.child(1).input_without_ignored()
                Dim params As vector(Of builders.parameter) = new_scope.params().unpack()
                Return logic_name.of_callee(function_name,
                                            n.child(0).input_without_ignored(),
                                            params,
                                            Function() As Boolean
                                                Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                Return l.of(n.child(gi)).build(fo)
                                            End Function,
                                            fo) AndAlso
                       o.append(new_scope.call_hierarchy().filter(
                                    logic_name.of_function(function_name, +params),
                                    AddressOf fo.str))
            End Using
        End Function
    End Class
End Class
