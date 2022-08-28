
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class define_t
        Private ReadOnly d As New unordered_set(Of String)()

        Public Sub define(ByVal s As String)
            assert(d.emplace(s).second())
        End Sub

        Public Function is_defined(ByVal s As String) As Boolean
            Return d.find(s) <> d.end()
        End Function

        Private NotInheritable Class ifndef_wrapped_impl
            Implements code_gen(Of WRITER)

            Private ReadOnly code_gen_of As Func(Of typed_node, code_gens(Of WRITER).code_gen_proxy)
            Private ReadOnly is_defined As Func(Of String, Boolean)

            Public Sub New(ByVal code_gen_of As Func(Of typed_node, code_gens(Of WRITER).code_gen_proxy),
                           ByVal is_defined As Func(Of String, Boolean))
                assert(Not code_gen_of Is Nothing)
                assert(Not is_defined Is Nothing)
                Me.code_gen_of = code_gen_of
                Me.is_defined = is_defined
            End Sub

            Private Function build(ByVal n As typed_node,
                                   ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
                assert(Not n Is Nothing)
                assert(n.child_count() >= 3)
                If is_defined(n.child(1).word().str()) Then
                    Return True
                End If
                For i As UInt32 = 2 To n.child_count() - uint32_2
                    If Not code_gen_of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
                Return True
            End Function
        End Class

        Private NotInheritable Class define_impl
            Implements code_gen(Of WRITER)

            Private ReadOnly define As Action(Of String)

            Public Sub New(ByVal define As Action(Of String))
                assert(Not define Is Nothing)
                Me.define = define
            End Sub

            Private Function build(ByVal n As typed_node,
                                   ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
                assert(Not n Is Nothing)
                assert(n.child_count() = 2)
                define(n.child(1).word().str())
                Return True
            End Function
        End Class

        Public NotInheritable Class code_gens
            Public Shared Function ifndef_wrapped(ByVal code_gen_of As Func(Of typed_node,
                                                                               code_gens(Of WRITER).code_gen_proxy),
                                                  ByVal defines As Func(Of define_t)) As Action(Of code_gens(Of WRITER))
                assert(Not defines Is Nothing)
                Return Sub(ByVal c As code_gens(Of WRITER))
                           assert(Not c Is Nothing)
                           c.register("ifndef-wrapped",
                                      New ifndef_wrapped_impl(code_gen_of,
                                                              Function(ByVal s As String) As Boolean
                                                                  Return defines().is_defined(s)
                                                              End Function))
                       End Sub
            End Function

            Public Shared Function define(ByVal defines As Func(Of define_t)) As Action(Of code_gens(Of WRITER))
                assert(Not defines Is Nothing)
                Return Sub(ByVal c As code_gens(Of WRITER))
                           assert(Not c Is Nothing)
                           c.register("define", New define_impl(Sub(ByVal s As String)
                                                                    defines().define(s)
                                                                End Sub))
                       End Sub
            End Function

            Private Sub New()
            End Sub
        End Class
    End Class
End Class
