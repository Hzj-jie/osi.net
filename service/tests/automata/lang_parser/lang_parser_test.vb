
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.utt
Imports osi.service.automata

Public Class lang_parser_test
    Private NotInheritable Class tree_node
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
                Return subnodes(CInt(i))
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            r.Append(this)
            If Not isemptyarray(subnodes) Then
                r.Append("{")
                For i As Int32 = 0 To array_size_i(subnodes) - 1
                    r.Append(subnodes(i))
                Next
                r.Append("}")
            End If
            Return Convert.ToString(r)
        End Function
    End Class

    Private ReadOnly lp As lang_parser

    Protected Sub New(ByVal lp As lang_parser)
        assert(lp IsNot Nothing)
        Me.lp = lp
    End Sub

    Private Shared Function node(ByVal name As String, ByVal subnodes() As tree_node) As tree_node
        Return New tree_node(name, subnodes)
    End Function

    Private Shared Function node(ByVal name As String) As tree_node
        Return New tree_node(name)
    End Function

    Private Shared Function node(ByVal subnodes() As tree_node) As tree_node
        Return New tree_node(subnodes)
    End Function

    Private Function assert_node(ByVal n As typed_node, ByVal t As tree_node) As Boolean
        If Not assertion.is_not_null(n) OrElse Not assertion.is_not_null(t) Then
            Return False
        End If
        If strsame(t.this, tree_node.root_name) Then
            If Not assertion.equal(n.type, typed_node.ROOT_TYPE) Then
                Return False
            End If
        Else
            If Not assertion.equal(t.this, n.type_name) Then
                Return False
            End If
        End If
        If assert(n.subnodes IsNot Nothing) AndAlso
           assertion.equal(n.subnodes.size(), array_size(t.subnodes), n.subnodes, " v.s. ", t.subnodes) Then
            Dim i As UInt32 = 0
            While i < array_size(t.subnodes)
                If Not assert_node(n.child(i), t(i)) Then
                    Return False
                End If
                i += uint32_1
            End While
        End If
        Return True
    End Function

    Private Function run_case(ByVal txt As String,
                              Optional ByVal tree As tree_node = Nothing) As Boolean
        Dim n As typed_node = Nothing
        If assertion.is_true(lp.parse(txt, root:=n)) Then
            If tree IsNot Nothing Then
                assert_node(n, tree)
            End If
        End If
        Return True
    End Function

    Private Function case1() As Boolean
        Return run_case("void main(string args) { return 0; }",
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

    Private Function case2() As Boolean
        Return run_case(strcat("int max(int x, int y) { ", newline.incode(),
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

    Private Function case3() As Boolean
        Return run_case(strcat("int f1(int x, int y) {", newline.incode(),
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

    Private Function parse_only_cases() As Boolean
        Return run_case(bytes_str(lang_parser_test_case0)) AndAlso
               run_case(bytes_str(lang_parser_test_case1))
    End Function

    Public Function run() As Boolean
        Return case1() AndAlso
               case2() AndAlso
               case3() AndAlso
               parse_only_cases()
    End Function

    Public Shared Function run_cases(ByVal f As _do(Of lang_parser, Boolean)) As Boolean
        assert(f IsNot Nothing)
        Dim lp As lang_parser = Nothing
        Return assertion.is_true(f(lp)) AndAlso (New lang_parser_test(lp).run())
    End Function
End Class
