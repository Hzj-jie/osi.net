
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class _namespace
        Implements code_gen(Of typed_node_writer)

        Private Const namespace_replacer As String = "__"

        Public NotInheritable Class bstyle_format
            Public Shared Function [of](ByVal i As String) As String
                assert(Not i.null_or_whitespace())
                Return streams.of(scope.current_namespace_t.of(i).Split("."c)).
                               map(Function(ByVal x As String) As String
                                       assert(Not x Is Nothing)
                                       If Not x.Contains(scope.current_namespace_t.namespace_separator) Then
                                           Return x
                                       End If
                                       Return scope.current_namespace_t.of(x).
                                                     Replace(scope.current_namespace_t.namespace_separator,
                                                             namespace_replacer).
                                                     Substring(namespace_replacer.Length())
                                   End Function).
                               collect_by(stream(Of String).collectors.to_str(".")).
                               ToString()
            End Function

            Public Shared Function [of](ByVal n As typed_node) As String
                assert(Not n Is Nothing)
                Return [of](n.input_without_ignored())
            End Function

            ' TODO: The use case of this function is not valid, may consider to remove it.
            Public Shared Function with_namespace(ByVal ns As String, ByVal i As String) As String
                Return [of](scope.current_namespace_t.with_namespace(ns, i))
            End Function

            ' TODO: This function should return the same string as the input "i" and is quite confusing, may consider to
            ' remove it.
            Public Shared Function in_global_namespace(ByVal i As String) As String
                Return [of](scope.current_namespace_t.with_global_namespace(i))
            End Function

            Public Shared Function operator_function_name(ByVal operator_name As String) As String
                assert(Not operator_name.null_or_whitespace())
                Return [of](_namespace.in_b2style_namespace(operator_name.Replace("-"c, "_"c)))
            End Function

            Public Shared Function [of](ByVal n As name_with_namespace) As String
                Return [of](_namespace.in_global_namespace(n))
            End Function

            Private Sub New()
            End Sub
        End Class

        Public Shared Function in_b2style_namespace(ByVal name As String) As String
            Return scope.current_namespace_t.with_global_namespace(
                      scope.current_namespace_t.with_namespace("b2style", name))
        End Function

        Public Shared Function in_global_namespace(ByVal n As name_with_namespace) As String
            Return scope.current_namespace_t.with_global_namespace(
                      scope.current_namespace_t.with_namespace(n.namespace(), n.name()))
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 4)
            Using scope.current().current_namespace().define(scope.current_namespace_t.of(n.child(1).word().str()))
                For i As UInt32 = 3 To n.child_count() - uint32_2
                    If Not code_gen_of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
            End Using
            Return True
        End Function
    End Class
End Class
