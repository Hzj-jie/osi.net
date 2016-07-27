
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.automata
Imports def_bool = System.Boolean
Imports def_int = System.Int32
Imports def_float = System.Double
Imports def_char = System.Char
Imports def_string = System.String

Namespace fullstack.syntax
    'a bare variable
    Public Class variable
        Public ReadOnly type As type
        Public ReadOnly value As Object
        'root is 0
        Public ReadOnly level As UInt32
        Public ReadOnly offset As UInt32

        Public Sub New(ByVal t As type,
                       ByVal value As Object,
                       ByVal level As UInt32,
                       ByVal offset As UInt32)
            assert(Not t Is Nothing)
            Me.type = t
            Me.value = value
            Me.level = level
            Me.offset = offset
        End Sub

        Private Shared Function parse_string(ByVal word As String, ByRef o As def_string) As Boolean
            assert(Not String.IsNullOrEmpty(word))
            Return word.strstartwith(character.quote) AndAlso
                   word.strendwith(character.quote) AndAlso
                   eva(o, strmid(word,
                                 strlen(character.quote),
                                 strlen(word) - strlen(character.quote) - strlen(character.quote)))
        End Function

        Private Shared Function parse_int(ByVal word As String, ByRef o As def_int) As Boolean
            assert(Not String.IsNullOrEmpty(word))
            Return def_int.TryParse(word, o)
        End Function

        Private Shared Function parse_float(ByVal word As String, ByRef o As def_float) As Boolean
            assert(Not String.IsNullOrEmpty(word))
            Return def_float.TryParse(word, o)
        End Function

        Private Shared Function parse_char(ByVal word As String, ByRef o As def_char) As Boolean
            assert(Not String.IsNullOrEmpty(word))
            Return word.strstartwith(character.single_quotation) AndAlso
                   word.strendwith(character.single_quotation) AndAlso
                   strlen(word) = strlen(character.single_quotation) +
                                  1 +
                                  strlen(character.single_quotation) AndAlso
                   eva(o, strmid(word,
                                 strlen(character.single_quotation),
                                 strlen(word) -
                                 strlen(character.single_quotation) -
                                 strlen(character.single_quotation))(0))
        End Function

        Private Shared Function parse_bool(ByVal word As String, ByRef o As def_bool) As Boolean
            assert(Not String.IsNullOrEmpty(word))
            Return def_bool.TryParse(word, o)
        End Function

        Public Shared Function parse(ByVal words() As lexer.typed_word,
                                     ByVal pos As UInt32,
                                     ByVal result As parser.syntax_stack,
                                     ByRef var As variable) As Boolean
            Dim s As def_string = Nothing
            Dim i As def_int = 0
            Dim f As def_float = 0
            Dim c As def_char = Nothing
            Dim b As def_bool = False
            If parse_string(words(pos).text(), s) Then
                var = result.domain_manager.domain().define(type.string, s)
            ElseIf parse_int(words(pos).text(), i) Then
                var = result.domain_manager.domain().define(type.int, i)
            ElseIf parse_float(words(pos).text(), f) Then
                var = result.domain_manager.domain().define(type.float, f)
            ElseIf parse_char(words(pos).text(), c) Then
                var = result.domain_manager.domain().define(type.char, f)
            ElseIf parse_bool(words(pos).text(), b) Then
                var = result.domain_manager.domain().define(type.bool, b)
            Else
                Return False
            End If
            Return True
        End Function

        Public Function execute(ByVal domain As executor.domain,
                                ByRef var As executor.variable) As Boolean
            assert(Not domain Is Nothing)
            var = New executor.variable(type, value)
            domain.define(var)
            Return True
        End Function
    End Class
End Namespace
