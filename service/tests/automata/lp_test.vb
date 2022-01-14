
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata

Partial Public Class lp_test
    Inherits [case]

    Private Class transition
        Public ReadOnly from As Int32
        Public ReadOnly variable As Int32
        Public ReadOnly [to] As Int32
        Public ReadOnly key_word As String

        Private Sub New(ByVal from As Int32,
                        ByVal variable As Int32,
                        ByVal [to] As Int32,
                        ByVal key_word As String)
            Me.from = from
            Me.variable = variable
            Me.to = [to]
            Me.key_word = key_word
        End Sub

        Public Sub New(ByVal words() As lexer.typed_word,
                       ByVal pos As UInt32,
                       ByVal from As Int32,
                       ByVal [to] As Int32)
            Me.New(from,
                   CInt(words(CInt(pos)).type),
                   [to],
                   cast(Of lexer.word)().from(words(CInt(pos))).text)
        End Sub

        Public Function assert_equal(ByVal from As Int32,
                                     ByVal [to] As Int32) As Boolean
            Return assertion.equal(from, Me.from) AndAlso
                   assertion.equal([to], Me.to)
        End Function

        Public Function assert_equal(ByVal from As Int32,
                                     ByVal [to] As Int32,
                                     ByVal key_word As String) As Boolean
            Return assert_equal(from, [to]) AndAlso
                   assertion.equal(key_word, Me.key_word)
        End Function

        Public Function assert_equal(ByVal from As Int32,
                                     ByVal varaible As Int32,
                                     ByVal [to] As Int32) As Boolean
            Return assert_equal(from, [to]) AndAlso
                   assertion.equal(varaible, Me.variable)
        End Function

        Public Function assert_equal(ByVal from As Int32,
                                     ByVal variable As Int32,
                                     ByVal [to] As Int32,
                                     ByVal key_word As String) As Boolean
            Return assert_equal(from, [to], key_word) AndAlso
                   assertion.equal(key_word, Me.key_word)
        End Function
    End Class

    Private Shared Function case1(ByVal p As lp(Of vector(Of transition))) As Boolean
        Dim r As lp(Of vector(Of transition)).result = Nothing
        r = p.execute("")
        assertion.is_true(r.has_error())
        assertion.is_true(r.lex_error)
        Return True
    End Function

    Private Shared Function case2(ByVal p As lp(Of vector(Of transition))) As Boolean
        Const subject As String = "i"
        Const predicate As String = "like"
        Const [object] As String = "orange"
        Dim r As lp(Of vector(Of transition)).result = Nothing
        r = p.execute(strcat(subject, character.blank, predicate, character.blank, [object]))
        assertion.is_false(r.has_error())
        Return (assertion.is_not_null(r.result) AndAlso
                assertion.equal(r.result.size(), CUInt(4)) AndAlso
                assertion.is_not_null(r.result(0)) AndAlso
                r.result(0).assert_equal(test_status_id.start,
                                         test_status_id.after_subject,
                                         subject) AndAlso
                r.result(1).assert_equal(test_status_id.after_subject,
                                         test_status_id.after_predicate,
                                         predicate) AndAlso
                r.result(2).assert_equal(test_status_id.after_predicate,
                                         test_status_id.after_object,
                                         [object]) AndAlso
                r.result(3).assert_equal(test_status_id.after_object,
                                         lexer.end_type,
                                         test_status_id.end)) OrElse
               True
    End Function

    Public Overrides Function run() As Boolean
        If write_syntax_file() Then
            Dim p As lp(Of vector(Of transition)) = Nothing
            p = lp(Of vector(Of transition)).ctor(Of lp_test)(syntax_file_name)
            Return assertion.is_not_null(p) AndAlso
                   (case1(p) AndAlso
                    case2(p))
        Else
            Return False
        End If
    End Function
End Class
