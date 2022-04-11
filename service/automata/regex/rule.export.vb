
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class rlexer
    Partial Public Class rule
        Public Function export() As exporter
            Dim o As exporter = Nothing
            assert(export(o))
            Return o
        End Function

        Public Function export(ByRef o As exporter) As Boolean
            Return exporter.create(Me, o)
        End Function

        Public NotInheritable Class exporter
            Public ReadOnly rlexer As rlexer
            Public ReadOnly macros As macros
            Public ReadOnly words() As regex
            Public ReadOnly type_choice As match_choice
            Public ReadOnly word_choice As match_choice
            Public ReadOnly str_type As const_array(Of String)

            Private Sub New(ByVal rlexer As rlexer,
                            ByVal macros As macros,
                            ByVal words() As regex,
                            ByVal type_choice As match_choice,
                            ByVal word_choice As match_choice,
                            ByVal str_type As const_array(Of String))
                assert(Not rlexer Is Nothing)
                Me.rlexer = rlexer
                assert(Not macros Is Nothing)
                Me.macros = macros
                assert(Not words Is Nothing)
                Me.words = words
                Me.type_choice = type_choice
                Me.word_choice = word_choice
                assert(Not str_type Is Nothing)
                Me.str_type = str_type
            End Sub

            Public Shared Function create(ByVal i As rule, ByRef o As exporter) As Boolean
                If i Is Nothing Then
                    Return False
                End If
                Dim macros As New macros()
                Dim type_choice As match_choice = Nothing
                Dim word_choice As match_choice = Nothing
                If Not i.macros.empty() Then
                    macros.define(i.macros)
                End If

                If i.type_choice Is Nothing Then
                    type_choice = match_choice.first_defined
                ElseIf Not enum_def.from(i.type_choice, type_choice) Then
                    raise_error(error_type.user, "failed to parse type_choice ", i.type_choice)
                    Return False
                End If

                If i.word_choice Is Nothing Then
                    word_choice = match_choice.greedy
                ElseIf Not enum_def.from(i.word_choice, word_choice) Then
                    raise_error(error_type.user, "failed to parse word_choice ", i.word_choice)
                    Return False
                End If

                Dim rlexer As New rlexer(type_choice, word_choice)

                Dim str_type As New vector(Of String)()
                Dim words() As regex = Nothing
                If Not i.words.empty() Then
                    ReDim words(CInt(i.words.size() - uint32_1))
                    For j As UInt32 = 0 To i.words.size() - uint32_1
                        Dim s As String = macros.expand(i.words(j).second)
                        If Not regex.create(s, words(CInt(j))) Then
                            raise_error(error_type.user, "failed to parse regex ", s)
                            Return False
                        End If
                        assert(rlexer.define(words(CInt(j))))
                        assert(Not String.IsNullOrEmpty(i.words(j).first))
                        str_type.emplace_back(i.words(j).first)
                    Next
                End If

                o = New exporter(rlexer, macros, words, type_choice, word_choice, const_array.of(+str_type))
                Return True
            End Function

            Public Function str_type_mapping() As unordered_map(Of String, UInt32)
                Return streams.of(str_type.as_array()).
                               with_index().
                               map(AddressOf tuple(Of UInt32, String).to_first_const_pair).
                               map(first_const_pair(Of UInt32, String).emplace_reverse).
                               collect_to(Of unordered_map(Of String, UInt32))()
            End Function
        End Class
    End Class
End Class
