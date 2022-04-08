
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
            Private ReadOnly str_type As unordered_map(Of String, UInt32)
            Private ReadOnly type_str As unordered_map(Of UInt32, String)

            Private Sub New(ByVal rlexer As rlexer,
                            ByVal macros As macros,
                            ByVal words() As regex,
                            ByVal type_choice As match_choice,
                            ByVal word_choice As match_choice,
                            ByVal str_type As unordered_map(Of String, UInt32),
                            ByVal type_str As unordered_map(Of UInt32, String))
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
                assert(Not type_str Is Nothing)
                Me.type_str = type_str
            End Sub

            Public Shared Function create(ByVal i As rule, ByRef o As exporter) As Boolean
                If i Is Nothing Then
                    Return False
                End If
                Dim macros As New macros()
                Dim type_choice As match_choice = Nothing
                Dim word_choice As match_choice = Nothing
                Dim str_type As New unordered_map(Of String, UInt32)()
                Dim type_str As New unordered_map(Of UInt32, String)()
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

                Dim words() As regex = Nothing
                If Not i.words.empty() Then
                    ReDim words(CInt(i.words.size() - uint32_1))
                    For j As UInt32 = 0 To i.words.size() - uint32_1
                        Dim s As String = Nothing
                        s = macros.expand(i.words(j).second)
                        If Not regex.create(s, words(CInt(j))) Then
                            raise_error(error_type.user, "failed to parse regex ", s)
                            Return False
                        End If
                        assert(rlexer.define(words(CInt(j))))
                        assert(Not String.IsNullOrEmpty(i.words(j).first))
                        str_type(i.words(j).first) = rlexer.regex_count() - uint32_1
                        type_str(rlexer.regex_count() - uint32_1) = i.words(j).first
                    Next
                End If

                o = New exporter(rlexer, macros, words, type_choice, word_choice, str_type, type_str)
                Return True
            End Function

            Public Function str_to_type(ByVal i As String, ByRef o As UInt32) As Boolean
                Dim it As unordered_map(Of String, UInt32).iterator = str_type.find(i)
                If it = str_type.end() Then
                    Return False
                End If
                o = (+it).second
                Return True
            End Function

            Public Function str_to_type(ByVal i As String) As UInt32
                Dim o As UInt32 = 0
                If str_to_type(i, o) Then
                    Return o
                End If
                Return typed_word.unknown_type
            End Function

            Public Function type_to_str(ByVal i As UInt32, ByRef o As String) As Boolean
                Dim it As unordered_map(Of UInt32, String).iterator = type_str.find(i)
                If it = type_str.end() Then
                    Return False
                End If
                o = (+it).second
                Return True
            End Function

            Public Function type_to_str(ByVal i As UInt32) As String
                Dim o As String = Nothing
                If type_to_str(i, o) Then
                    Return o
                End If
                Return Nothing
            End Function

            Public Function str_type_mapping() As unordered_map(Of String, UInt32)
                Return copy(str_type)
            End Function

            Public Function type_str_mapping() As unordered_map(Of UInt32, String)
                Return copy(type_str)
            End Function

            Public Sub types_mapping(ByRef str_type As unordered_map(Of String, UInt32),
                                     ByRef type_str As unordered_map(Of UInt32, String))
                str_type = str_type_mapping()
                type_str = type_str_mapping()
            End Sub

            Public Function types_to_strs(ByVal v As vector(Of typed_word), ByRef o As vector(Of String)) As Boolean
                If v.null_or_empty() Then
                    Return True
                End If
                o.renew()
                o.resize(v.size())
                For i As UInt32 = 0 To v.size() - uint32_1
                    Dim s As String = Nothing
                    If Not v(i) Is Nothing AndAlso type_to_str(v(i).type, s) Then
                        o(i) = s
                    Else
                        Return False
                    End If
                Next
                Return True
            End Function

            Public Function strs_to_types(ByVal v As vector(Of String), ByRef o As vector(Of UInt32)) As Boolean
                If v.null_or_empty() Then
                    Return True
                End If
                o.renew()
                o.resize(v.size())
                For i As UInt32 = 0 To v.size() - uint32_1
                    Dim u As UInt32 = Nothing
                    If str_to_type(v(i), u) Then
                        o(i) = u
                    Else
                        Return False
                    End If
                Next
                Return True
            End Function
        End Class
    End Class
End Class
