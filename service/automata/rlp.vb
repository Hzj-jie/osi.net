
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class rlp
    Implements lang_parser

    Private ReadOnly r As rlexer.rule.exporter
    Private ReadOnly s As syntaxer.rule.exporter

    Private Sub New(ByVal r As rlexer.rule.exporter, ByVal s As syntaxer.rule.exporter)
        assert(r IsNot Nothing)
        assert(s IsNot Nothing)
        Me.r = r
        Me.s = s
    End Sub

    Public Function parse(ByVal txt As String,
                          Optional ByRef words As vector(Of typed_word) = Nothing,
                          Optional ByRef root As typed_node = Nothing) As Boolean Implements lang_parser.parse
        Return r.rlexer.match(txt, words) AndAlso
               s.syntaxer.match(words, root)
    End Function

    Public Shared Function create(ByVal r As rlexer.rule.exporter,
                                  ByVal s As syntaxer.rule.exporter,
                                  ByRef o As rlp) As Boolean
        If r Is Nothing OrElse s Is Nothing Then
            Return False
        End If
        o = New rlp(r, s)
        Return True
    End Function

    Public Shared Function create(ByVal r As rlexer.rule, ByVal s As syntaxer.rule, ByRef o As rlp) As Boolean
        If r Is Nothing OrElse s Is Nothing Then
            Return False
        End If
        Return assert(create(r.export(), s.export(), o))
    End Function

    Private Shared Function create(ByVal rl As Func(Of rlexer.rule, Boolean),
                                   ByVal sl As Func(Of syntaxer.rule, Boolean),
                                   ByRef o As rlp) As Boolean
        assert(rl IsNot Nothing)
        assert(sl IsNot Nothing)
        Dim r As New rlexer.rule()
        If Not rl(r) Then
            Return False
        End If
        Dim re As rlexer.rule.exporter = r.export()
        Dim s As New syntaxer.rule(re.str_type_mapping())
        If Not sl(s) Then
            Return False
        End If
        Return assert(create(re, s.export(), o))
    End Function

    Public Shared Function create_from_file(ByVal rlexer_rule_file As String,
                                            ByVal syntaxer_rule_file As String,
                                            ByRef o As rlp) As Boolean
        Return create(Function(r As rlexer.rule) As Boolean
                          assert(r IsNot Nothing)
                          Return r.parse_file(rlexer_rule_file)
                      End Function,
                      Function(s As syntaxer.rule) As Boolean
                          assert(s IsNot Nothing)
                          Return s.parse_file(syntaxer_rule_file)
                      End Function,
                      o)
    End Function

    Public Shared Function create_from_content(ByVal rlexer_rule As String,
                                               ByVal syntaxer_rule As String,
                                               ByRef o As rlp) As Boolean
        Return create(Function(r As rlexer.rule) As Boolean
                          assert(r IsNot Nothing)
                          Return r.parse_content(rlexer_rule)
                      End Function,
                      Function(s As syntaxer.rule) As Boolean
                          assert(s IsNot Nothing)
                          Return s.parse_content(syntaxer_rule)
                      End Function,
                      o)
    End Function

    Public Shared Function create(ByVal rlexer_rules() As String,
                                  ByVal syntaxer_rules() As String,
                                  ByRef o As rlp) As Boolean
        Return create(Function(r As rlexer.rule) As Boolean
                          assert(r IsNot Nothing)
                          Return r.parse(rlexer_rules)
                      End Function,
                      Function(s As syntaxer.rule) As Boolean
                          assert(s IsNot Nothing)
                          Return s.parse(syntaxer_rules)
                      End Function,
                      o)
    End Function
End Class
