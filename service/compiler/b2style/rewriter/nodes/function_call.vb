
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
    Public NotInheritable Class function_call
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of function_call)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3 OrElse n.child_count() = 4)
            If n.child(0).word().str().Contains(".") Then
                Dim dot_pos As Int32 = n.child(0).word().str().LastIndexOf(".")
                ' dot is not allowed to be the first character.
                assert(dot_pos > 0)
                ' TODO: This should be handled by nlexer_rule.
                If dot_pos = n.child(0).word().str().Length() - 1 Then
                    raise_error(error_type.user,
                                "Name [",
                                n.child(0).word().str(),
                                "] is not allowed to be suffixed by dot.")
                    Return False
                End If
                o.append(n.child(0).word().str().Substring(dot_pos + 1)).
                  append("(").
                  append(n.child(0).word().str().Substring(0, dot_pos))
                If n.child_count() = 4 Then
                    o.append(", ")
                    If Not l.of(n.child(2)).build(o) Then
                        Return False
                    End If
                End If
                o.append(")")
                Return True
            End If
            Return streams.range(0, n.child_count()).
                           map(Function(ByVal i As Int32) As typed_node
                                   Return n.child(CUInt(i))
                               End Function).
                           map(Function(ByVal node As typed_node) As Boolean
                                   Return l.of(node).build(o)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true)
        End Function
    End Class
End Class
