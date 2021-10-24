
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class logic_gens
    Public NotInheritable Class [default]
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly f As Func(Of [default], typed_node, writer, Boolean)

        Private Sub New(ByVal l As logic_gens, ByVal f As Func(Of [default], typed_node, writer, Boolean))
            MyBase.New(l)
            assert(Not f Is Nothing)
            Me.f = f
        End Sub

        Private Shared Function registerer(ByVal s As String,
                                           ByVal f As Func(Of [default], typed_node, writer, Boolean)) _
                                          As Action(Of logic_gens)
            Return Sub(ByVal b As logic_gens)
                       assert(Not b Is Nothing)
                       b.register(s, New [default](b, f))
                   End Sub
        End Function

        Public Shared Function [of](ByVal s As String,
                                    ByVal ParamArray selected_children() As UInt32) As Action(Of logic_gens)
            assert(Not selected_children.null_or_empty())
            Return registerer(s, Function(ByVal this As [default],
                                          ByVal n As typed_node,
                                          ByVal o As writer) As Boolean
                                     For i As Int32 = 0 To selected_children.Length() - 1
                                         assert(n.child_count() > selected_children(i))
                                         If Not this.l.of(n.child(selected_children(i))).build(o) Then
                                             Return False
                                         End If
                                     Next
                                     Return True
                                 End Function)
        End Function

        Public Shared Function of_first_child(ByVal s As String) As Action(Of logic_gens)
            Return [of](s, 0)
        End Function

        Public Shared Function of_only_child(ByVal s As String) As Action(Of logic_gens)
            Return registerer(s,
                              Function(ByVal this As [default],
                                       ByVal n As typed_node,
                                       ByVal o As writer) As Boolean
                                  Return this.l.of(n.child()).build(o)
                              End Function)
        End Function

        Public Shared Function of_all_children(ByVal s As String) As Action(Of logic_gens)
            Return registerer(s,
                              Function(ByVal this As [default],
                                       ByVal n As typed_node,
                                       ByVal o As writer) As Boolean
                                  Dim i As UInt32 = 1
                                  While i < n.child_count() - uint32_1
                                      If Not this.l.of(n.child(i)).build(o) Then
                                          Return False
                                      End If
                                      i += uint32_1
                                  End While
                                  Return True
                              End Function)
        End Function

        Public Shared Function of_only_child_with_wrapper(Of T As IDisposable) _
                                                         (ByVal w As Func(Of T),
                                                          ByVal s As String) As Action(Of logic_gens)
            assert(Not w Is Nothing)
            Return registerer(s,
                              Function(ByVal this As [default],
                                       ByVal n As typed_node,
                                       ByVal o As writer) As Boolean
                                  Using w()
                                      Return this.l.of(n.child()).build(o)
                                  End Using
                              End Function)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return f(Me, n, o)
        End Function
    End Class
End Class
