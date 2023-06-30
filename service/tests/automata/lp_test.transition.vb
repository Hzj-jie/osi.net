
Imports osi.root.formation
Imports osi.service.automata

Partial Public Class lp_test
    'the internal status id depends on the implementation
    Private Class test_status_id
        Public Const start As Int32 = 0
        Public Const after_subject As Int32 = 1
        Public Const after_predicate As Int32 = 2
        Public Const after_object As Int32 = 3
        Public Const [end] As Int32 = 4
    End Class

    Private Shared Function start_to_after_subject(ByVal words() As lexer.typed_word,
                                                   ByVal pos As UInt32,
                                                   ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.start,
                                           test_status_id.after_subject))
        Return True
    End Function

    Private Shared Function after_subject_to_after_predicate(ByVal words() As lexer.typed_word,
                                                             ByVal pos As UInt32,
                                                             ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.after_subject,
                                           test_status_id.after_predicate))
        Return True
    End Function

    Private Shared Function after_predicate_to_after_predicate(ByVal words() As lexer.typed_word,
                                                               ByVal pos As UInt32,
                                                               ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.after_predicate,
                                           test_status_id.after_predicate))
        Return True
    End Function

    Private Shared Function after_predicate_to_after_object(ByVal words() As lexer.typed_word,
                                                            ByVal pos As UInt32,
                                                            ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.after_predicate,
                                           test_status_id.after_object))
        Return True
    End Function

    Private Shared Function after_predicate_to_after_object_unknown(ByVal words() As lexer.typed_word,
                                                                    ByVal pos As UInt32,
                                                                    ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.after_predicate,
                                           test_status_id.after_object))
        Return True
    End Function

    Private Shared Function after_object_to_end(ByVal words() As lexer.typed_word,
                                                ByVal pos As UInt32,
                                                ByVal result As vector(Of transition)) As Boolean
        result.emplace_back(New transition(words,
                                           pos,
                                           test_status_id.after_object,
                                           test_status_id.end))
        Return True
    End Function
End Class
