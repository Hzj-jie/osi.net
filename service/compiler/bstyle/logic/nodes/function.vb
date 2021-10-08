
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [function]
        Inherits logic_gen_wrapper_with_parameters
        Implements logic_gen

        Private ReadOnly s As New write_scoped(Of ref)()

        Private Sub New(ByVal b As logic_gens, ByVal l As parameters_t)
            MyBase.New(b, l)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal l As parameters_t)
            assert(Not b Is Nothing)
            assert(Not l Is Nothing)
            b.register(New [function](b, l))
        End Sub

        Public NotInheritable Class ref
            Private ReadOnly ta As type_alias
            Private ReadOnly params As vector(Of builders.parameter)
            Private ReadOnly n As typed_node

            Public Sub New(ByVal ta As type_alias,
                           ByVal params As vector(Of builders.parameter),
                           ByVal n As typed_node)
                assert(Not ta Is Nothing)
                assert(Not params Is Nothing)
                assert(Not n Is Nothing)
                assert(n.child_count() = 5 OrElse n.child_count() = 6)
                Me.ta = ta
                Me.params = params
                Me.n = n
            End Sub

            Public Function allow_return_value() As Boolean
                Return Not strsame(n.child(0).word().str(), types.void)
            End Function

            Public Function name() As String
                Return function_name.of_function(ta, n.child(1).word().str(), params)
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
                function_name.of_callee(ta,
                                        n.child(1).word().str(),
                                        n.child(0).word().str(),
                                        +params,
                                        Function() As Boolean
                                            Using s.push(New ref(ta, +params, n))
                                                Dim gi As UInt32 = 0
                                                gi = CUInt(If(has_paramlist, 5, 4))
                                                Return l.of(n.child(gi)).build(o)
                                            End Using
                                        End Function).to(o)
                Return True
            End Using
        End Function
    End Class
End Class
