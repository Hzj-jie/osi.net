
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.utils

Public Class parser(Of _MAX_TYPE As _int64, RESULT_T)
    Public Const start_status As UInt32 = automata.dfa(Of lexer.typed_word, 
                                                          lexer.MAX_TYPE(Of _MAX_TYPE), 
                                                          lexer.typed_word_to_uint32, 
                                                          RESULT_T).start_status
    Public Const end_status As UInt32 = automata.dfa(Of lexer.typed_word, 
                                                        lexer.MAX_TYPE(Of _MAX_TYPE), 
                                                        lexer.typed_word_to_uint32, 
                                                        RESULT_T).end_status
    Public Const first_user_status As UInt32 = automata.dfa(Of lexer.typed_word, 
                                                               lexer.MAX_TYPE(Of _MAX_TYPE), 
                                                               lexer.typed_word_to_uint32, 
                                                               RESULT_T).first_user_status

    Private ReadOnly dfa As dfa(Of lexer.typed_word, 
                                   lexer.MAX_TYPE(Of _MAX_TYPE), 
                                   lexer.typed_word_to_uint32, 
                                   RESULT_T)

    Public Sub New()
        dfa = New dfa(Of lexer.typed_word, lexer.MAX_TYPE(Of _MAX_TYPE), lexer.typed_word_to_uint32, RESULT_T)()
    End Sub

    Public Function define(ByVal current_status As UInt32,
                           ByVal word As lexer.typed_word,
                           ByVal next_status As UInt32,
                           ByVal action As Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean)) As Boolean
        Return dfa.insert(current_status, word, next_status, action)
    End Function

    Public Function define(ByVal current_status As UInt32,
                           ByVal word As UInt32,
                           ByVal next_status As UInt32,
                           ByVal action As Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean)) As Boolean
        Return define(current_status, New lexer.typed_word(word), next_status, action)
    End Function

    Public Function define(ByVal current_status As UInt32,
                           ByVal word As lexer.typed_word,
                           ByVal next_status As UInt32) As Boolean
        Return dfa.insert(current_status, word, next_status)
    End Function

    Public Function define(ByVal current_status As UInt32,
                           ByVal word As UInt32,
                           ByVal next_status As UInt32) As Boolean
        Return define(current_status, New lexer.typed_word(word), next_status)
    End Function

    Private Function define(ByVal words() As lexer.typed_word,
                            ByVal d As Func(Of lexer.typed_word, Boolean)) As Boolean
        assert(Not d Is Nothing)
        For i As Int32 = 0 To array_size(words) - 1
            If Not d(words(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function define(ByVal current_status As UInt32,
                           ByVal next_status As UInt32,
                           ByVal action As Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean),
                           ByVal ParamArray words() As lexer.typed_word) As Boolean
        Return define(words, Function(x) define(current_status, x, next_status, action))
    End Function

    Public Function define(ByVal current_status As UInt32,
                           ByVal next_status As UInt32,
                           ByVal ParamArray words() As lexer.typed_word) As Boolean
        Return define(words, Function(x) define(current_status, x, next_status))
    End Function

    Public Function parse(ByVal words() As lexer.word, ByVal o As RESULT_T) As Boolean
        Dim r As Boolean = False
        Return dfa.parse(words, o, r) = array_size(words) AndAlso r
    End Function

    Public Function parse(ByVal words() As lexer.word, ByRef start As UInt32, ByVal o As RESULT_T) As Boolean
        Dim r As Boolean = False
        Dim l As UInt32 = 0
        l = dfa.parse(words, start, o, r)
        If r Then
            start += l
            Return True
        Else
            Return False
        End If
    End Function
End Class
