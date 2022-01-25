
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public NotInheritable Class syntax_test2
        Inherits [case]

        Private Enum types As UInt32
            [function] = 1
            blank
            name
            paramlist
            empty_paramlist
            paragraph
            multi_sentence_paragraph
            start_bracket
            param_with_comma
            param
            end_bracket
            comma
            start_paragraph
            sentence
            end_paragraph
            value_definition
            value_clause
            condition
            semi_colon
            assignment
            KW_if
            value
            else_condition
            KW_else
            comparasion
            value_without_comparasion
            less_than
            great_than
            less_or_equal
            great_or_equal
            equal
            function_call
            valuelist
            empty_valuelist
            value_with_comma
        End Enum

        Private ReadOnly c As New syntax_collection()

        Private Function build_syntax() As syntax
            c.clear()
            Dim [function] As syntax = Nothing
            [function] = New syntax(c,
                                    types.function,
                                    matching_creator.create(c, types.name),
                                    matching_creator.create(c, types.name),
                                    matching_creator.create(c, types.start_bracket),
                                    New matching_group(c,
                                                       New matching_delegate(c, types.paramlist),
                                                       New matching_delegate(c, types.empty_paramlist)),
                                    matching_creator.create(c, types.end_bracket),
                                    New matching_delegate(c, types.multi_sentence_paragraph))
            assert(c.set([function]))
            assert(c.set(New syntax(c,
                                    types.paramlist,
                                    New multi_matching_group(c, New matching_delegate(c, types.param_with_comma)),
                                    New matching_delegate(c, types.param))))
            assert(c.set(New syntax(c,
                                    types.empty_paramlist,
                                    matching_creator.create(c))))
            assert(c.set(New syntax(c,
                                    types.param_with_comma,
                                    New matching_delegate(c, types.param),
                                    matching_creator.create(c, types.comma))))
            assert(c.set(New syntax(c,
                                    types.param,
                                    matching_creator.create(c, types.name),
                                    matching_creator.create(c, types.name))))
            assert(c.set(New syntax(c,
                                    types.paragraph,
                                    New matching_group(c,
                                                       New matching_delegate(c, types.sentence),
                                                       New matching_delegate(c, types.multi_sentence_paragraph)))))
            assert(c.set(New syntax(c,
                                    types.multi_sentence_paragraph,
                                    matching_creator.create(c, types.start_paragraph),
                                    New multi_matching_group(c, New matching_delegate(c, types.sentence)),
                                    matching_creator.create(c, types.end_paragraph))))
            assert(c.set(New syntax(c,
                                    types.sentence,
                                    New matching_group(c,
                                                       New matching_delegate(c, types.value_definition),
                                                       New matching_delegate(c, types.value_clause),
                                                       New matching_delegate(c, types.condition)),
                                    matching_creator.create(c, types.semi_colon))))
            assert(c.set(New syntax(c,
                                    types.value_definition,
                                    matching_creator.create(c, types.name),
                                    matching_creator.create(c, types.name))))
            assert(c.set(New syntax(c,
                                    types.value_clause,
                                    New matching_delegate(c, types.value),
                                    matching_creator.create(c, types.assignment),
                                    New matching_delegate(c, types.value))))
            assert(c.set(New syntax(c,
                                    types.condition,
                                    matching_creator.create(c, types.KW_if),
                                    matching_creator.create(c, types.start_bracket),
                                    New matching_delegate(c, types.value),
                                    matching_creator.create(c, types.end_bracket),
                                    New matching_delegate(c, types.paragraph),
                                    New optional_matching_group(c, New matching_delegate(c, types.else_condition)))))
            assert(c.set(New syntax(c,
                                    types.else_condition,
                                    matching_creator.create(c, types.KW_else),
                                    New matching_delegate(c, types.paragraph))))
            assert(c.set(New syntax(c,
                                    types.value,
                                    New matching_group(c,
                                                       matching_creator.create(c, types.name),
                                                       New matching_delegate(c, types.comparasion),
                                                       New matching_delegate(c, types.function_call)))))
            assert(c.set(New syntax(c,
                                    types.comparasion,
                                    New matching_delegate(c, types.value_without_comparasion),
                                    matching_creator.create(c,
                                                            types.less_than,
                                                            types.great_than,
                                                            types.less_or_equal,
                                                            types.great_or_equal,
                                                            types.equal),
                                    New matching_delegate(c, types.value))))
            assert(c.set(New syntax(c,
                                    types.value_without_comparasion,
                                    New matching_group(c,
                                                       matching_creator.create(c, types.name),
                                                       New matching_delegate(c, types.function_call)))))
            assert(c.set(New syntax(c,
                                    types.function_call,
                                    matching_creator.create(c, types.name),
                                    matching_creator.create(c, types.start_bracket),
                                    New matching_group(c,
                                                       New matching_delegate(c, types.valuelist),
                                                       New matching_delegate(c, types.empty_valuelist)),
                                    matching_creator.create(c, types.end_bracket))))
            assert(c.set(New syntax(c,
                                    types.valuelist,
                                    New multi_matching_group(c, New matching_delegate(c, types.value_with_comma)),
                                    New matching_delegate(c, types.value))))
            assert(c.set(New syntax(c,
                                    types.empty_valuelist,
                                    matching_creator.create(c))))
            assert(c.set(New syntax(c,
                                    types.value_with_comma,
                                    New matching_delegate(c, types.value),
                                    matching_creator.create(c, types.comma))))
            Return [function]
        End Function

        Private Shared Function assert_node(ByVal n As typed_node,
                                            ByVal id As UInt32,
                                            ByVal type As UInt32,
                                            ByVal word_start As UInt32,
                                            ByVal word_end As UInt32) As Boolean
            assert(Not n Is Nothing)
            Return assertion.more(n.subnodes.size(), id) AndAlso
                   assertion.equal(n.subnodes(id).type, type) AndAlso
                   assertion.equal(n.subnodes(id).word_start, word_start) AndAlso
                   assertion.equal(n.subnodes(id).word_end, word_end)
        End Function

        Private Shared Function assert_node(ByVal n As typed_node,
                                            ByVal id As UInt32,
                                            ByVal type As UInt32,
                                            ByVal start As UInt32) As Boolean
            Return assert_node(n, id, type, start, start + uint32_1)
        End Function

        Public Overrides Function run() As Boolean
            Dim s As syntax = build_syntax()
            Dim v As vector(Of typed_word) = typed_word.fakes(types.name,
                                                              types.blank,
                                                              types.blank,
                                                              types.name,
                                                              types.start_bracket,
                                                              types.blank,
                                                              types.name,
                                                              types.blank,
                                                              types.name,
                                                              types.comma,
                                                              types.blank,
                                                              types.blank,
                                                              types.name,
                                                              types.name,
                                                              types.end_bracket,
                                                              types.blank,
                                                              types.start_paragraph,
                                                              types.name,
                                                              types.assignment,
                                                              types.name,
                                                              types.semi_colon,
                                                              types.end_paragraph)
            Dim r As one_of(Of matching.result, matching.failure) = s.match(v, 0)
            If Not assertion.is_true(r.is_first()) Then
                Return False
            End If
            assertion.equal(r.first().pos, v.size())
            Dim n As typed_node = typed_node.of_root(v)
            n.attach(r.first().nodes)
            If assert_node(n, 0, types.function, 0, 22) Then
                n = n.subnodes(0)
                If assert_node(n, 0, types.name, 0) AndAlso
                   assert_node(n, 1, types.name, 3) AndAlso
                   assert_node(n, 2, types.start_bracket, 4) AndAlso
                   assert_node(n, 3, types.paramlist, 6, 14) AndAlso
                   assert_node(n, 4, types.end_bracket, 14) AndAlso
                   assert_node(n, 5, types.multi_sentence_paragraph, 16, 22) Then
                    If assert_node(n.subnodes(3), 0, types.param_with_comma, 6, 10) AndAlso
                       assert_node(n.subnodes(3), 1, types.param, 12, 14) Then
                        If assert_node(n.subnodes(3).subnodes(0), 0, types.param, 6, 9) AndAlso
                           assert_node(n.subnodes(3).subnodes(0), 1, types.comma, 9) Then
                            assert_node(n.subnodes(3).subnodes(0).subnodes(0), 0, types.name, 6)
                            assert_node(n.subnodes(3).subnodes(0).subnodes(0), 1, types.name, 8)
                        End If
                        assert_node(n.subnodes(3).subnodes(1), 0, types.name, 12)
                        assert_node(n.subnodes(3).subnodes(1), 1, types.name, 13)
                    End If
                    n = n.subnodes(5)
                    If assert_node(n, 0, types.start_paragraph, 16) AndAlso
                       assert_node(n, 1, types.sentence, 17, 21) AndAlso
                       assert_node(n, 2, types.end_paragraph, 21) Then
                        n = n.subnodes(1)
                        If assert_node(n, 0, types.value_clause, 17, 20) AndAlso
                           assert_node(n, 1, types.semi_colon, 20) Then
                            n = n.subnodes(0)
                            If assert_node(n, 0, types.value, 17) AndAlso
                               assert_node(n, 1, types.assignment, 18) AndAlso
                               assert_node(n, 2, types.value, 19) Then
                                assert_node(n.subnodes(0), 0, types.name, 17)
                                assert_node(n.subnodes(2), 0, types.name, 19)
                            End If
                        End If
                    End If
                End If
            End If

            v = typed_word.fakes(types.name,
                                 types.blank,
                                 types.blank,
                                 types.name,
                                 types.start_bracket,
                                 types.blank,
                                 types.name,
                                 types.blank,
                                 types.name,
                                 types.comma,
                                 types.blank,
                                 types.blank,
                                 types.name,
                                 types.name,
                                 types.end_bracket,
                                 types.blank,
                                 types.start_paragraph,
                                 types.name,
                                 types.equal,
                                 types.name,
                                 types.semi_colon,
                                 types.end_paragraph)
            assertion.is_false(s.match(v, 0).is_first())
            Return True
        End Function
    End Class
End Namespace
