
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Public NotInheritable Class nlp
    Implements lang_parser

    Private ReadOnly n As nlexer
    Private ReadOnly s As syntaxer

    Private Sub New(ByVal n As nlexer, ByVal s As syntaxer)
        assert(n IsNot Nothing)
        assert(s IsNot Nothing)
        Me.n = n
        Me.s = s
    End Sub

    Public Function parse(ByVal txt As String,
                          Optional ByRef words As vector(Of typed_word) = Nothing,
                          Optional ByRef root As typed_node = Nothing) As Boolean Implements lang_parser.parse
        Return n.match(txt, words) AndAlso
               s.match(words, root)
    End Function

    Public Shared Function [of](ByVal n As nlexer, ByVal s As syntaxer, ByRef o As nlp) As Boolean
        If n Is Nothing OrElse s Is Nothing Then
            Return False
        End If
        o = New nlp(n, s)
        Return True
    End Function

    Private Shared Function create(ByVal n As _do(Of nlexer, Boolean),
                                   ByVal s As Func(Of syntaxer.rule, Boolean),
                                   ByRef o As nlp) As Boolean
        assert(n IsNot Nothing)
        assert(s IsNot Nothing)
        Dim i As nlexer = Nothing
        If Not n(i) Then
            Return False
        End If
        Dim j As New syntaxer.rule(i.str_type_mapping())
        If Not s(j) Then
            Return False
        End If
        Return [of](i, j.export().syntaxer, o)
    End Function

    Public Shared Function of_file(ByVal n As String, ByVal s As String, ByRef o As nlp) As Boolean
        Return create(Function(ByRef i As nlexer) As Boolean
                          Return nlexer.of_file(n, i)
                      End Function,
                      Function(ByVal i As syntaxer.rule) As Boolean
                          Return i.parse_file(s)
                      End Function,
                      o)
    End Function

    Public Shared Function of_file(ByVal n As String, ByVal s As String) As nlp
        Dim o As nlp = Nothing
        assert(of_file(n, s, o))
        Return o
    End Function

    Public Shared Function [of](ByVal n As String, ByVal s As String, ByRef o As nlp) As Boolean
        Return create(Function(ByRef i As nlexer) As Boolean
                          Return nlexer.of(n, i)
                      End Function,
                    Function(ByVal i As syntaxer.rule) As Boolean
                        Return i.parse_content(s)
                    End Function,
                    o)
    End Function

    Public Shared Function [of](ByVal n As String, ByVal s As String) As nlp
        Dim o As nlp = Nothing
        assert([of](n, s, o))
        Return o
    End Function

    Public Shared Function [of](ByVal n() As String, ByVal s() As String, ByRef o As nlp) As Boolean
        Return create(Function(ByRef i As nlexer) As Boolean
                          Return nlexer.of(n, i)
                      End Function,
                      Function(ByVal i As syntaxer.rule) As Boolean
                          Return i.parse(s)
                      End Function,
                      o)
    End Function

    Public Shared Function [of](ByVal n() As String, ByVal s() As String) As nlp
        Dim o As nlp = Nothing
        assert([of](n, s, o))
        Return o
    End Function
End Class
