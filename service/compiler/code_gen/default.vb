
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public Class code_gens(Of WRITER)
    Public NotInheritable Class [default]
        Inherits code_gen_wrapper(Of WRITER)
        Implements code_gen(Of WRITER)

        Private ReadOnly f As Func(Of code_gens(Of WRITER), typed_node, WRITER, Boolean)

        Private Sub New(ByVal l As code_gens(Of WRITER),
                        ByVal f As Func(Of code_gens(Of WRITER), typed_node, WRITER, Boolean))
            MyBase.New(l)
            assert(Not f Is Nothing)
            Me.f = f
        End Sub

        Private Shared Function registerer(ByVal s As String,
                                           ByVal f As Func(Of code_gens(Of WRITER), typed_node, WRITER, Boolean)) _
                                          As Action(Of code_gens(Of WRITER))
            Return Sub(ByVal b As code_gens(Of WRITER))
                       assert(Not b Is Nothing)
                       b.register(s, New [default](b, f))
                   End Sub
        End Function

        Public Shared Function [of](ByVal s As String,
                                    ByVal ParamArray selected_children() As UInt32) As Action(Of code_gens(Of WRITER))
            assert(Not selected_children.null_or_empty())
            Return registerer(s,
                              Function(ByVal this As code_gens(Of WRITER),
                                       ByVal n As typed_node,
                                       ByVal o As WRITER) As Boolean
                                  assert(Not this Is Nothing)
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  For i As Int32 = 0 To selected_children.Length() - 1
                                      assert(n.child_count() > selected_children(i))
                                      If Not this.of(n.child(selected_children(i))).build(o) Then
                                          Return False
                                      End If
                                  Next
                                  Return True
                              End Function)
        End Function

        Public Shared Function of_first_child(ByVal s As String) As Action(Of code_gens(Of WRITER))
            Return [of](s, 0)
        End Function

        Public Shared Function of_only_child(ByVal s As String) As Action(Of code_gens(Of WRITER))
            Return registerer(s,
                              Function(ByVal this As code_gens(Of WRITER),
                                       ByVal n As typed_node,
                                       ByVal o As WRITER) As Boolean
                                  assert(Not this Is Nothing)
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  Return this.of(n.child()).build(o)
                              End Function)
        End Function

        Public Shared Function of_all_children(ByVal s As String) As Action(Of code_gens(Of WRITER))
            Return registerer(s,
                              Function(ByVal this As code_gens(Of WRITER),
                                       ByVal n As typed_node,
                                       ByVal o As WRITER) As Boolean
                                  assert(Not this Is Nothing)
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  Dim i As UInt32 = 0
                                  While i < n.child_count()
                                      If Not this.of(n.child(i)).build(o) Then
                                          Return False
                                      End If
                                      i += uint32_1
                                  End While
                                  Return True
                              End Function)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return f(l, n, o)
        End Function
    End Class
End Class
