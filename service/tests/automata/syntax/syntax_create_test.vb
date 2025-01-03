
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
    Public NotInheritable Class syntax_create_test
        Inherits [case]

        Private NotInheritable Class syntax_case
            Public ReadOnly str As String
            Public ReadOnly result As Boolean
            Public ReadOnly tokens() As String
            Public ReadOnly syntaxes() As String
            Public ReadOnly matches() As matching

            Public Sub New(ByVal str As String)
                Me.str = str
                Me.result = False
                Me.tokens = Nothing
                Me.syntaxes = Nothing
                Me.matches = Nothing
            End Sub

            Public Sub New(ByVal str As String,
                           ByVal tokens() As String,
                           ByVal syntaxes() As String,
                           ByVal matches() As matching)
                Me.str = str
                Me.result = True
                Me.tokens = tokens
                Me.syntaxes = syntaxes
                Me.matches = matches
            End Sub
        End Class

        Private Shared ReadOnly c As syntax_collection
        Private Shared ReadOnly cases() As syntax_case

        Shared Sub New()
            c = New syntax_collection()
            cases = {New syntax_case("[abc"),
                     New syntax_case("a,b,c d-e-f"),
                     New syntax_case("a-b d-f", {"a-b", "d-f"},
                                     {},
                                     {New single_matching(c, 0),
                                     New single_matching(c, 1)}),
                     New syntax_case("a-b d-f",
                                     {},
                                     {},
                                     {New fake_matching_delegate(c, 0),
                                     New fake_matching_delegate(c, 1)}),
                     New syntax_case("a-b d-f",
                                     {"a-b"},
                                     {"d-f"},
                                     {New single_matching(c, 0),
                                     New fake_matching_delegate(c, 1)}),
                     New syntax_case("[a, b, c] d",
                                     {"a", "b", "c", "d"},
                                     {},
                                     {New matching_group(c, 0, 1, 2),
                                     New single_matching(c, 3)})}
        End Sub

        Private Shared Sub merge_str_type(ByVal tokens() As String,
                                          ByVal result As map(Of String, UInt32),
                                          ByRef next_type As UInt32)
            assert(Not result Is Nothing)
            If Not isemptyarray(tokens) Then
                For j As Int32 = 0 To array_size_i(tokens) - 1
                    assert(result.find(tokens(j)) = result.end())
                    result(tokens(j)) = next_type
                    next_type += uint32_1
                Next
            End If
        End Sub

        Public Overrides Function run() As Boolean
            assert(Not isemptyarray(cases))
            For i As Int32 = 0 To array_size_i(cases) - 1
                assert(Not cases(i) Is Nothing)
                If cases(i).result Then
                    Dim collection As syntax_collection = Nothing
                    collection = New syntax_collection(cases(i).tokens, cases(i).syntaxes)
                    Dim o As syntax = Nothing
                    assertion.is_true(syntax.create(cases(i).str, collection, o))
                    If assertion.is_not_null(o) AndAlso
                       assertion.equal(array_size(+o), array_size(cases(i).matches)) AndAlso
                       Not isemptyarray(+o) Then
                        For j As Int32 = 0 To array_size_i(+o) - 1
                            ' fake_matching_delegate.CompareTo should be selected
                            assertion.equal(cases(i).matches(j), (+o)(j))
                        Next
                    End If
                Else
                    assertion.is_false(syntax.create(cases(i).str, New syntax_collection(), Nothing))
                End If
            Next
            Return True
        End Function
    End Class
End Namespace
