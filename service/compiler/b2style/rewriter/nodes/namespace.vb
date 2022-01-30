
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
    Public NotInheritable Class _namespace
        Implements code_gen(Of typed_node_writer)

        Private Const namespace_separator As String = "::"
        Private Const namespace_replacer As String = "__"
        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Shared Function [of](ByVal i As String) As String
            Return with_namespace(scope.current().current_namespace().name(), i)
        End Function

        Public NotInheritable Class bstyle_format
            Public Shared Function [of](ByVal i As String) As String
                assert(Not i.null_or_whitespace())
                Return streams.of(_namespace.of(i).Split("."c)).
                               map(Function(ByVal x As String) As String
                                       assert(Not x Is Nothing)
                                       If Not x.Contains(namespace_separator) Then
                                           Return x
                                       End If
                                       Return _namespace.of(x).
                                                     Replace(namespace_separator, namespace_replacer).
                                                     Substring(namespace_replacer.Length())
                                   End Function).
                               collect_by(stream(Of String).collectors.to_str(".")).
                               ToString()
            End Function

            Public Shared Function in_global_namespace(ByVal i As String) As String
                Return [of](with_namespace("", i))
            End Function

            Public Shared Function operator_function_name(ByVal operator_name As String) As String
                assert(Not operator_name.null_or_whitespace())
                Return [of](with_namespace("", with_namespace("b2style", operator_name.Replace("-"c, "_"c))))
            End Function

            Private Sub New()
            End Sub
        End Class

        Public Shared Function with_global_namespace(ByVal n As String) As String
            Return with_namespace("", n)
        End Function

        Private Shared Function with_namespace(ByVal n As String, ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return strcat(n, namespace_separator, i)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 4)
            Using scope.current().current_namespace().define([of](n.child(1).word().str()))
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
