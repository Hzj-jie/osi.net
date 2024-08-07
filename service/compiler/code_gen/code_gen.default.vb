
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Public NotInheritable Class code_gen_delegate(Of WRITER As New)
    Implements code_gen(Of WRITER)

    Private ReadOnly f As Func(Of typed_node, WRITER, Boolean)

    Private Sub New(ByVal f As Func(Of typed_node, WRITER, Boolean))
        assert(Not f Is Nothing)
        Me.f = f
    End Sub

    Public Shared Function [of](ByVal name As String,
                                ByVal f As Func(Of typed_node, WRITER, Boolean)) _
                               As Action(Of code_gens(Of WRITER))
        Return [of](name,
                    Function(ByVal l As code_gens(Of WRITER), ByVal n As typed_node, ByVal o As WRITER) As Boolean
                        Return f(n, o)
                    End Function)
    End Function

    Public Shared Function [of](ByVal name As String,
                                ByVal f As Func(Of code_gens(Of WRITER), typed_node, WRITER, Boolean)) _
                               As Action(Of code_gens(Of WRITER))
        Return Sub(ByVal b As code_gens(Of WRITER))
                   assert(Not b Is Nothing)
                   b.register(name,
                              New code_gen_delegate(Of WRITER)(
                                  Function(ByVal n As typed_node, ByVal o As WRITER) As Boolean
                                      Return f(b, n, o)
                                  End Function))
               End Sub
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        Return f(n, o)
    End Function
End Class

Public NotInheritable Class code_gen
    Public Shared Function of_ignore(Of WRITER As New)(ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   assert(Not this Is Nothing)
                                                   assert(Not n Is Nothing)
                                                   assert(Not o Is Nothing)
                                                   Return True
                                               End Function)
    End Function

    Public Shared Function of_children(Of WRITER As New) _
                                      (ByVal name As String,
                                       ByVal ParamArray selected_children() As UInt32) _
                                      As Action(Of code_gens(Of WRITER))
        assert(Not selected_children.null_or_empty())
        Return code_gen_delegate(Of WRITER).of(name,
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

    Public Shared Function of_first_child(Of WRITER As New)(ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return of_children(Of WRITER)(name, 0)
    End Function

    Public Shared Function of_ignore_last_child(Of WRITER As New) _
                                               (ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   assert(Not this Is Nothing)
                                                   assert(Not n Is Nothing)
                                                   assert(Not o Is Nothing)
                                                   If n.child_count() <= 1 Then
                                                       Return True
                                                   End If
                                                   Dim i As UInt32 = 0
                                                   While i < n.child_count() - uint32_1
                                                       If Not this.of(n.child(i)).build(o) Then
                                                           Return False
                                                       End If
                                                       i += uint32_1
                                                   End While
                                                   Return True
                                               End Function)
    End Function

    Public Shared Function of_only_child(Of WRITER As New)(ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   assert(Not this Is Nothing)
                                                   assert(Not n Is Nothing)
                                                   assert(Not o Is Nothing)
                                                   Return this.of(n.child()).build(o)
                                               End Function)
    End Function

    Public Shared Function of_all_children_with_precondition(Of WRITER As New) _
                                                            (ByVal w As Func(Of typed_node, Boolean),
                                                             ByVal name As String) As Action(Of code_gens(Of WRITER))
        assert(Not w Is Nothing)
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   If Not w(n) Then
                                                       Return False
                                                   End If
                                                   Return this.of_all_children(n).build(o)
                                               End Function)
    End Function

    Public Shared Function of_all_children_with_precondition(Of WRITER As New) _
                                                            (ByVal name As String,
                                                             ByVal ParamArray ws() As Func(Of typed_node, Boolean)) _
                                                            As Action(Of code_gens(Of WRITER))
        assert(Not ws.null_or_empty())
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   For Each w As Func(Of typed_node, Boolean) In ws
                                                       assert(Not w Is Nothing)
                                                       If Not w(n) Then
                                                           Return False
                                                       End If
                                                   Next
                                                   Return this.of_all_children(n).build(o)
                                               End Function)
    End Function

    Public Shared Function of_all_children_with_wrapper(Of T As IDisposable, WRITER As New) _
                                                       (ByVal w As Func(Of T),
                                                        ByVal name As String) As Action(Of code_gens(Of WRITER))
        assert(Not w Is Nothing)
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   Using w()
                                                       Return this.of_all_children(n).build(o)
                                                   End Using
                                               End Function)
    End Function

    Public Shared Function of_all_children(Of WRITER As New)(ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(name,
                                               Function(ByVal this As code_gens(Of WRITER),
                                                        ByVal n As typed_node,
                                                        ByVal o As WRITER) As Boolean
                                                   Return this.of_all_children(n).build(o)
                                               End Function)
    End Function

    Public Shared Function of_leaf_node(ByVal name As String) As Action(Of code_gens(Of typed_node_writer))
        Return code_gen_delegate(Of typed_node_writer).of(
                   name,
                   Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                       assert(Not n Is Nothing)
                       assert(Not o Is Nothing)
                       assert(n.leaf())
                       Return o.append(n)
                   End Function)
    End Function

    Public Shared Function of_only_descendant_str(Of WRITER As {lazy_list_writer, New}) _
                                                 (ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(
                   name,
                   Function(ByVal n As typed_node, ByVal o As WRITER) As Boolean
                       assert(Not n Is Nothing)
                       assert(Not o Is Nothing)
                       Return o.append(n.only_descendant_str())
                   End Function)
    End Function

    Public Shared Function of_input(Of WRITER As {lazy_list_writer, New}) _
                                   (ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(
                   name,
                   Function(ByVal n As typed_node, ByVal o As WRITER) As Boolean
                       assert(Not n Is Nothing)
                       assert(Not o Is Nothing)
                       Return o.append(n.input())
                   End Function)
    End Function

    Public Shared Function of_input_without_ignored(Of WRITER As {lazy_list_writer, New}) _
                                                   (ByVal name As String) As Action(Of code_gens(Of WRITER))
        Return code_gen_delegate(Of WRITER).of(
                   name,
                   Function(ByVal n As typed_node, ByVal o As WRITER) As Boolean
                       assert(Not n Is Nothing)
                       assert(Not o Is Nothing)
                       Return o.append(n.input_without_ignored())
                   End Function)
    End Function

    Private Sub New()
    End Sub
End Class
