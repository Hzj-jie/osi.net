
Imports osi.service.automata

Namespace fullstack.parser
    Public Class syntax_stack
        Public ReadOnly domain_manager As syntax.domain_manager

        Public Sub New()
            domain_manager = New syntax.domain_manager()
        End Sub

        Private Shared Function void_function_start(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function types_function_start(ByVal words() As lexer.typed_word,
                                                     ByVal pos As UInt32,
                                                     ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_function_name(ByVal words() As lexer.typed_word,
                                                   ByVal pos As UInt32,
                                                   ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function function_parameter_list(ByVal words() As lexer.typed_word,
                                                        ByVal pos As UInt32,
                                                        ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_parameter_type(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_parameter_name(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_one_parameter(ByVal words() As lexer.typed_word,
                                                   ByVal pos As UInt32,
                                                   ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_parameter_list(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function start_paragraph(ByVal words() As lexer.typed_word,
                                                ByVal pos As UInt32,
                                                ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function pass_first_token(ByVal words() As lexer.typed_word,
                                                 ByVal pos As UInt32,
                                                 ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function assignment_before_expression(ByVal words() As lexer.typed_word,
                                                             ByVal pos As UInt32,
                                                             ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function function_call_before_first_parameter(ByVal words() As lexer.typed_word,
                                                                     ByVal pos As UInt32,
                                                                     ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function expression_left_bracket(ByVal words() As lexer.typed_word,
                                                        ByVal pos As UInt32,
                                                        ByVal result As syntax_stack) As Boolean

        End Function

        Private Shared Function expression_first_token(ByVal words() As lexer.typed_word,
                                                       ByVal pos As UInt32,
                                                       ByVal result As syntax_stack) As Boolean

        End Function
    End Class
End Namespace
