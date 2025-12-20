
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class b3style
    Private NotInheritable Class _function
        Implements code_gen(Of logic_writer)

        Private Shared remove_unused_functions As argument(Of Boolean)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Using new_scope As scope = scope.current().start_scope()
                Dim fo As New logic_writer()
                Dim has_paramlist As Boolean = strsame(n.child(3).type_name, "paramlist")
                If has_paramlist Then
                    If Not code_gen_of(n.child(3)).build(fo) Then
                        Return False
                    End If
                End If
                Dim function_name As String = scope.fully_qualified_function_name.of(n.child(1))
                Dim params As vector(Of builders.parameter) = new_scope.params().unpack()
                Return logic_name.of_callee(function_name,
                                            scope.type_name.of(n.child(0)),
                                            params,
                                            Function() As Boolean
                                                Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                Return code_gen_of(n.child(gi)).build(fo)
                                            End Function,
                                            fo) AndAlso
                       o.append(new_scope.call_hierarchy().filter(
                                    logic_name.of_function(function_name, +params),
                                    AddressOf fo.str))
            End Using
        End Function

        Private Shared Function param_types(ByVal n As typed_node) As vector(Of String)
            Return New vector(Of String)()

            ' Following implementation does not work, b2style has no information about the types, the
            ' function_call_with_template cannot create correct parameter types.
#If 0 Then
            assert(Not n Is Nothing)
            If n.child_count() = 5 Then
                Return New vector(Of String)()
            End If
            assert(n.child_count() = 6)
            Return n.child(3).
                     children().
                     map(Function(ByVal node As typed_node) As String
                             assert(Not node Is Nothing)
                             If node.type_name.Equals("param-with-comma") Then
                                 node = node.child(0)
                             End If
                             assert(node.type_name.Equals("param"))
                             Return node.child(0).input_without_ignored()
                         End Function).
                     collect_to(Of vector(Of String))()
#End If
        End Function
    End Class
End Class
