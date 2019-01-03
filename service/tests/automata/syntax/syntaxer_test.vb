
Imports System.IO
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.automata
Imports rrule = osi.service.automata.rlexer.rule
Imports srule = osi.service.automata.syntaxer.rule
Imports rlex = osi.service.automata.rlexer
Imports synt = osi.service.automata.syntaxer

Namespace syntaxer
    Public Class syntaxer_test
        Inherits [case]

        Private Class tree_node
            Public Const root_name As String = "ROOT"
            Public ReadOnly this As String
            Public ReadOnly subnodes() As tree_node

            Public Sub New(ByVal this As String, ByVal subnodes() As tree_node)
                Me.this = this
                Me.subnodes = subnodes
            End Sub

            Public Sub New(ByVal this As String)
                Me.New(this, Nothing)
            End Sub

            Public Sub New(ByVal subnodes() As tree_node)
                Me.New(root_name, subnodes)
            End Sub

            Default Public ReadOnly Property node(ByVal i As UInt32) As tree_node
                Get
                    assert(i < array_size(subnodes))
                    Return subnodes(i)
                End Get
            End Property
        End Class

        Private Shared Function node(ByVal name As String, ByVal subnodes() As tree_node) As tree_node
            Return New tree_node(name, subnodes)
        End Function

        Private Shared Function node(ByVal name As String) As tree_node
            Return New tree_node(name)
        End Function

        Private Shared Function node(ByVal subnodes() As tree_node) As tree_node
            Return New tree_node(subnodes)
        End Function

        Private Shared Function assert_node(ByVal s As synt, ByVal n As typed_node, ByVal t As tree_node) As Boolean
            assert(Not s Is Nothing)
            If assertion.is_not_null(n) AndAlso assertion.is_not_null(t) Then
                If strsame(t.this, tree_node.root_name) Then
                    If Not assertion.equal(n.type, typed_node.ROOT_TYPE) Then
                        Return False
                    End If
                Else
                    Dim o As UInt32 = 0
                    If Not assertion.is_true(s.type_id(t.this, o)) OrElse
                        Not assertion.equal(n.type, o) Then
                        Return False
                    End If
                End If
                If assert(Not n.subnodes Is Nothing) AndAlso
                   assertion.equal(n.subnodes.size(), array_size(t.subnodes)) Then
                    Dim i As UInt32 = 0
                    While i < array_size(t.subnodes)
                        If Not assert_node(s, n(i), t(i)) Then
                            Return False
                        End If
                        i += 1
                    End While
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Private Shared Function run_case(ByVal r As rlex,
                                         ByVal s As synt,
                                         ByVal rlp As rlp,
                                         ByVal txt As String,
                                         Optional ByVal tree As tree_node = Nothing) As Boolean
            assert(Not r Is Nothing)
            assert(Not s Is Nothing)
            assert(Not rlp Is Nothing)
            Dim n As typed_node = Nothing
            Dim w As vector(Of typed_word) = Nothing
            If assertion.is_true(r.match(txt, w)) Then
                If assertion.is_true(s.match(w, n)) Then
                    If Not tree Is Nothing Then
                        assert_node(s, n, tree)
                    End If
                End If
            End If
            If assertion.is_true(rlp.parse(txt, root:=n)) Then
                If Not tree Is Nothing Then
                    assert_node(s, n, tree)
                End If
            End If
            Return True
        End Function

        Private Shared Function case1(ByVal r As rlex, ByVal s As synt, ByVal rlp As rlp) As Boolean
            Return run_case(r,
                            s,
                            rlp,
                            "void main(string args) { return 0; }",
                            node({node("function",
                                       {node("name"),
                                        node("name"),
                                        node("start-bracket"),
                                        node("paramlist",
                                             {node("param",
                                                   {node("name"),
                                                    node("name")
                                                   })
                                             }),
                                        node("end-bracket"),
                                        node("multi-sentence-paragraph",
                                             {node("start-paragraph"),
                                              node("sentence",
                                                   {node("sentence-with-semi-colon",
                                                         {node("return-clause",
                                                               {node("KW-return"),
                                                                node("value",
                                                                     {node("integer")
                                                                     })
                                                               }),
                                                          node("semi-colon")
                                                         })
                                                   }),
                                              node("end-paragraph")
                                             })
                                       })
                                 }))
            Return True
        End Function

        Private Shared Function case2(ByVal r As rlex, ByVal s As synt, ByVal rlp As rlp) As Boolean
            Return run_case(r,
                            s,
                            rlp,
                            strcat("int max(int x, int y) { ", newline.incode(),
                                   "if (x > y) return x;", newline.incode(),
                                   "else return y ;  }"),
                            node({node("function",
                                       {node("name"),
                                        node("name"),
                                        node("start-bracket"),
                                        node("paramlist",
                                             {node("param-with-comma",
                                                   {node("param",
                                                         {node("name"),
                                                          node("name")
                                                         }),
                                                    node("comma")
                                                   }),
                                              node("param",
                                                   {node("name"),
                                                    node("name")
                                                   })
                                             }),
                                        node("end-bracket"),
                                        node("multi-sentence-paragraph",
                                             {node("start-paragraph"),
                                              node("sentence",
                                                   {node("sentence-without-semi-colon",
                                                         {node("condition",
                                                               {node("KW-if"),
                                                                node("start-bracket"),
                                                                node("value",
                                                                     {node("comparasion",
                                                                           {node("value-without-comparasion",
                                                                                 {node("name")
                                                                                 }),
                                                                            node("great-than"),
                                                                            node("value",
                                                                                 {node("name")
                                                                                 })
                                                                           })
                                                                     }),
                                                                node("end-bracket"),
                                                                node("paragraph",
                                                                     {node("sentence",
                                                                           {node("sentence-with-semi-colon",
                                                                                 {node("return-clause",
                                                                                       {node("KW-return"),
                                                                                        node("value",
                                                                                             {node("name")
                                                                                             })
                                                                                       }),
                                                                                  node("semi-colon")
                                                                                 })
                                                                           })
                                                                     }),
                                                                node("else-condition",
                                                                     {node("KW-else"),
                                                                      node("paragraph",
                                                                           {node("sentence",
                                                                                 {node("sentence-with-semi-colon",
                                                                                       {node("return-clause",
                                                                                             {node("KW-return"),
                                                                                              node("value",
                                                                                                   {node("name")
                                                                                                   })
                                                                                             }),
                                                                                        node("semi-colon")
                                                                                       })
                                                                                 })
                                                                           })
                                                                     })
                                                               })
                                                         })
                                                   }),
                                              node("end-paragraph")
                                             })
                                       })
                                 }))
        End Function

        Private Shared Function case3(ByVal r As rlex, ByVal s As synt, ByVal rlp As rlp) As Boolean
            Return run_case(r,
                            s,
                            rlp,
                            strcat("int f1(int x, int y) {", newline.incode(),
                                   "if( x > 0) return x;", newline.incode(),
                                   "else return y; }", newline.incode(),
                                   "int f2()", newline.incode(),
                                   "{", newline.incode(),
                                   "    return f1(100,10);", newline.incode(),
                                   "}"),
                            node({node("function",
                                       {node("name"),
                                        node("name"),
                                        node("start-bracket"),
                                        node("paramlist",
                                             {node("param-with-comma",
                                                   {node("param",
                                                         {node("name"),
                                                          node("name")
                                                         }),
                                                    node("comma")
                                                   }),
                                              node("param",
                                                   {node("name"),
                                                    node("name")
                                                   })
                                             }),
                                        node("end-bracket"),
                                        node("multi-sentence-paragraph",
                                             {node("start-paragraph"),
                                              node("sentence",
                                                   {node("sentence-without-semi-colon",
                                                         {node("condition",
                                                               {node("KW-if"),
                                                                node("start-bracket"),
                                                                node("value",
                                                                     {node("comparasion",
                                                                           {node("value-without-comparasion",
                                                                                 {node("name")
                                                                                 }),
                                                                            node("great-than"),
                                                                            node("value",
                                                                                 {node("integer")
                                                                                 })
                                                                           })
                                                                     }),
                                                                node("end-bracket"),
                                                                node("paragraph",
                                                                     {node("sentence",
                                                                           {node("sentence-with-semi-colon",
                                                                                 {node("return-clause",
                                                                                       {node("KW-return"),
                                                                                        node("value",
                                                                                             {node("name")
                                                                                             })
                                                                                       }),
                                                                                  node("semi-colon")
                                                                                 })
                                                                           })
                                                                     }),
                                                                node("else-condition",
                                                                     {node("KW-else"),
                                                                      node("paragraph",
                                                                           {node("sentence",
                                                                                 {node("sentence-with-semi-colon",
                                                                                       {node("return-clause",
                                                                                             {node("KW-return"),
                                                                                              node("value",
                                                                                                   {node("name")
                                                                                                   })
                                                                                             }),
                                                                                        node("semi-colon")
                                                                                       })
                                                                                 })
                                                                           })
                                                                     })
                                                               })
                                                         })
                                                   }),
                                              node("end-paragraph")
                                             })
                                       }),
                                  node("function",
                                       {node("name"),
                                        node("name"),
                                        node("start-bracket"),
                                        node("end-bracket"),
                                        node("multi-sentence-paragraph",
                                             {node("start-paragraph"),
                                              node("sentence",
                                                   {node("sentence-with-semi-colon",
                                                         {node("return-clause",
                                                               {node("KW-return"),
                                                                node("value",
                                                                     {node("function-call",
                                                                           {node("name"),
                                                                            node("start-bracket"),
                                                                            node("value-list",
                                                                                 {node("value-with-comma",
                                                                                       {node("value",
                                                                                             {node("integer")
                                                                                             }),
                                                                                        node("comma")
                                                                                       }),
                                                                                  node("value",
                                                                                       {node("integer")
                                                                                       })
                                                                                 }),
                                                                            node("end-bracket")
                                                                           })
                                                                     })
                                                               }),
                                                          node("semi-colon")
                                                         })
                                                   }),
                                              node("end-paragraph")
                                             })
                                       })
                                 }))
        End Function

        Private Shared Function parse_only_cases(ByVal r As rlex, ByVal s As synt, ByVal rlp As rlp) As Boolean
            Return run_case(r, s, rlp, bytes_str(syntaxer_test_case0)) AndAlso
                   run_case(r, s, rlp, bytes_str(syntaxer_test_case1))
        End Function

        Private Shared Function run_case(ByVal rrf As String, ByVal srf As String) As Boolean
            Dim rr As rrule = Nothing
            rr = New rrule()
            If assertion.is_true(rr.parse_file(rrf)) Then
                Dim re As rrule.exporter = Nothing
                If assertion.is_true(rr.export(re)) AndAlso assertion.is_not_null(re) Then
                    Dim sr As srule = Nothing
                    sr = New srule(re.str_type_mapping())
                    If assertion.is_true(sr.parse_file(srf)) Then
                        Dim se As srule.exporter = Nothing
                        se = sr.export()
                        Dim rlp As rlp = Nothing
                        If assertion.is_true(rlp.create_from_file(rrf, srf, rlp)) Then
                            Return case1(re.rlexer, se.syntaxer, rlp) AndAlso
                                   case2(re.rlexer, se.syntaxer, rlp) AndAlso
                                   case3(re.rlexer, se.syntaxer, rlp) AndAlso
                                   parse_only_cases(re.rlexer, se.syntaxer, rlp)
                        End If
                    End If
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return run_case(syntaxer_test_rule_files.rlexer, syntaxer_test_rule_files.syntaxer) AndAlso
                   run_case(syntaxer_test_rule_files.rlexer2, syntaxer_test_rule_files.syntaxer)
        End Function
    End Class
End Namespace
