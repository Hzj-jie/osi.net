
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
        Implements logic_gen

        Private ReadOnly s As New write_scoped(Of ref)()

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [function])()
        End Sub

        Public NotInheritable Class ref
            Private ReadOnly params As vector(Of builders.parameter)
            Private ReadOnly n As typed_node

            Public Sub New(ByVal params As vector(Of builders.parameter),
                           ByVal n As typed_node)
                assert(Not params Is Nothing)
                assert(Not n Is Nothing)
                assert(n.child_count() = 5 OrElse n.child_count() = 6)
                Me.params = params
                Me.n = n
            End Sub

            Public Function allow_return_value() As Boolean
                Return Not strsame(scope.current().type_alias()(n.child(0).word().str()), types.zero_type)
            End Function

            Public Function name() As String
                Return function_name.of_function(n.child(1).word().str(), params)
            End Function
        End Class

        Public Function target() As ref
            Return s.current()
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim has_paramlist As Boolean = False
            has_paramlist = strsame(n.child(3).type_name, "paramlist")
            If has_paramlist Then
                If Not l.of(n.child(3)).build(o) Then
                    Return False
                End If
            Else
                logic_gen_of(Of paramlist)().empty_paramlist()
            End If
            Using params As read_scoped(Of vector(Of builders.parameter)).ref =
                      code_gen_of(Of paramlist)().current_target()
                function_name.of_callee(n.child(1).word().str(),
                                        n.child(0).word().str(),
                                        +params,
                                        Function() As Boolean
                                            Using s.push(New ref(+params, n))
                                                Dim gi As UInt32 = CUInt(If(has_paramlist, 5, 4))
                                                Return l.of(n.child(gi)).build(o)
                                            End Using
                                        End Function).
                              to(o)
                Return True
            End Using
        End Function
    End Class
End Class
