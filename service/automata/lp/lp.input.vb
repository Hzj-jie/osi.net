
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils

Partial Public Class lp(Of MAX_TYPE As _int64, RESULT_T)
    Private NotInheritable Class input
        Private _use_default_separators As Boolean
        Private _ignore_separators As Boolean
        Private _method_type As String
        Public ReadOnly types As map(Of String, pair(Of vector(Of String), Boolean))
        Public ReadOnly statuses As map(Of String, map(Of String, pair(Of String, String)))

        Public Sub New()
            types = New map(Of String, pair(Of vector(Of String), Boolean))()
            statuses = New map(Of String, map(Of String, pair(Of String, String)))()
            _use_default_separators = True
            _ignore_separators = True
        End Sub

        Public Function use_default_separators() As Boolean
            Return _use_default_separators
        End Function

        Public Function ignore_separators() As Boolean
            Return _ignore_separators
        End Function

        Public Function method_type() As String
            Return _method_type
        End Function

        Private Shared Function parse_section_name(ByVal i As String) As String
            assert(Not i.null_or_empty())
            assert(Not i.strstartwith(value_start))
            Return i.Split(section_name_separators)(0)
        End Function

        Private Shared Function parse_sections(
                                    ByVal inputs As vector(Of String),
                                    ByRef sections As vector(Of pair(Of String, vector(Of String)))) As Boolean
            If inputs.null_or_empty() Then
                Return False
            End If
            sections.renew()
            For i As UInt32 = 0 To inputs.size() - uint32_1
                If Not inputs(i).null_or_whitespace() AndAlso
                       Not inputs(i).strstartwith(comment_start) Then
                    If inputs(i).strstartwith(value_start) OrElse
                           inputs(i).strstartwith(value_start2) Then
                        If sections.empty() Then
                            Return False
                        Else
                            Dim s As String = Nothing
                            If inputs(i).strstartwith(value_start) Then
                                s = strmid(inputs(i), strlen(value_start))
                            ElseIf inputs(i).strstartwith(value_start2) Then
                                s = strmid(inputs(i), strlen(value_start2))
                            Else
                                assert(False)
                            End If
                            sections.back().second.emplace_back(s)
                        End If
                    Else
                        sections.emplace_back(pair.of(parse_section_name(inputs(i)),
                                                            New vector(Of String)()))
                    End If
                End If
            Next
            Return True
        End Function

        Private Function parse_method_type(ByVal s As vector(Of String)) As Boolean
            Return Not s Is Nothing AndAlso
                   (s.empty() OrElse
                    (s.size() = 1 AndAlso eva(_method_type, s(0))))
        End Function

        Private Function parse_key_words(ByVal s As vector(Of String)) As Boolean
            If s Is Nothing Then
                Return False
            End If
            If s.empty() Then
                Return True
            End If
            For i As UInt32 = 0 To s.size() - uint32_1
                assert(Not s(i).null_or_empty())
                Dim ss() As String = s(i).Split(value_separators, StringSplitOptions.RemoveEmptyEntries)
                If array_size(ss) <= 1 Then
                    Return False
                End If
                Dim type_name As String = Nothing
                Dim ignore As Boolean = False
                If ss(0).strstartwith(type_ignore_mask) Then
                    ignore = True
                    type_name = strmid(ss(0), strlen(type_ignore_mask))
                Else
                    ignore = False
                    type_name = ss(0)
                End If
                If type_name.null_or_empty() Then
                    Return False
                End If
                For j As Int32 = 1 To array_size_i(ss) - 1
                    ss(j).c_unescape(ss(j))
                Next
                types(type_name).first.renew()
                assert(types(type_name).first.emplace_back(ss, 1))
                types(type_name).second = ignore
            Next
            Return True
        End Function

        Private Function parse_transitions(ByVal s As vector(Of String)) As Boolean
            If s Is Nothing Then
                Return False
            End If
            If s.empty() Then
                Return True
            End If
            For i As UInt32 = 0 To s.size() - uint32_1
                assert(Not s(i).null_or_empty())
                Dim ss() As String = s(i).Split(value_separators, StringSplitOptions.RemoveEmptyEntries)
                If array_size(ss) < 3 OrElse array_size(ss) > 4 Then
                    Return False
                End If
                Dim action As String = Nothing
                action = If(array_size(ss) = 3, Nothing, ss(3))
                statuses(ss(0))(ss(1)) = pair.of(ss(2), action)
            Next
            Return True
        End Function

        Private Function parse_default_separators(ByVal s As vector(Of String)) As Boolean
            If s Is Nothing Then
                Return False
            End If
            If s.empty() Then
                Return True
            End If
            If s.size() <> 1 Then
                Return False
            End If
            _use_default_separators = False
            _ignore_separators = False
            Dim ss() As String = Nothing
            ss = s(0).Split(default_separators_section_separators)
            For i As Int32 = 0 To array_size_i(ss) - 1
                If strsame(ss(i), use_default_separators_mask) Then
                    _use_default_separators = True
                ElseIf strsame(ss(i), ignore_separators_mask) Then
                    _ignore_separators = True
                End If
            Next
            Return True
        End Function

        Public Function parse(ByVal inputs As vector(Of String)) As Boolean
            Dim sections As vector(Of pair(Of String, vector(Of String))) = Nothing
            If Not parse_sections(inputs, sections) Then
                Return False
            End If
            assert(Not sections Is Nothing)
            If sections.empty() Then
                Return True
            End If
            For i As UInt32 = 0 To sections.size() - uint32_1
                Select Case sections(i).first
                    Case method_type_section_name
                        If Not parse_method_type(sections(i).second) Then
                            Return False
                        End If
                    Case key_words_section_name
                        If Not parse_key_words(sections(i).second) Then
                            Return False
                        End If
                    Case transitions_section_name
                        If Not parse_transitions(sections(i).second) Then
                            Return False
                        End If
                    Case default_separators_section_name
                        If Not parse_default_separators(sections(i).second) Then
                            Return False
                        End If
                    Case Else
                        Return False
                End Select
            Next
            Return True
        End Function
    End Class
End Class
