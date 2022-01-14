
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class namespace_
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        Private Const namespace_separator As String = "::"
        Private Const namespace_replacer As String = "__"

        <inject_constructor>
        Public Sub New(ByVal i As code_gens(Of typed_node_writer))
            MyBase.New(i)
        End Sub

        Private Shared Function full_name(ByVal i As String) As String
            Return with_namespace(scope.current().current_namespace().name(), i)
        End Function

        Public Shared Function bstyle_format_in_global_namespace(ByVal i As String) As String
            Return bstyle_format(with_namespace(empty_string, i))
        End Function

        Public Shared Function bstyle_format_in_root_namespace(ByVal n As String, ByVal i As String) As String
            Return bstyle_format(with_namespace(empty_string, with_namespace(n, i)))
        End Function

        Public Shared Function with_global_namespace(ByVal n As String) As String
            Return with_namespace(empty_string, n)
        End Function

        Private Shared Function with_namespace(ByVal n As String, ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return strcat(n, namespace_separator, i)
        End Function

        Public Shared Function bstyle_format(ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            Return streams.of(full_name(i).Split("."c)).
                           map(Function(ByVal x As String) As String
                                   assert(Not x Is Nothing)
                                   If Not x.Contains(namespace_separator) Then
                                       Return x
                                   End If
                                   Return full_name(x).Replace(namespace_separator, namespace_replacer).
                                                       Substring(namespace_replacer.Length())
                               End Function).
                           collect_by(stream(Of String).collectors.to_str(".")).
                           ToString()
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 4)
            Using New scope_wrapper()
                scope.current().current_namespace().define(full_name(n.child(1).word().str()))
                For i As UInt32 = 3 To n.child_count() - uint32_2
                    If Not l.of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
            End Using
            Return True
        End Function
    End Class
End Class
