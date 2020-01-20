
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports envs = osi.root.envs

Partial Public NotInheritable Class syntaxer
    Public MustInherit Class matching
        Implements IComparable, IComparable(Of matching)

        Protected ReadOnly c As syntax_collection

        Protected Sub New(ByVal c As syntax_collection)
            assert(Not c Is Nothing)
            Me.c = c
        End Sub

        Public NotInheritable Class result
            Public ReadOnly pos As UInt32
            Public ReadOnly nodes As vector(Of typed_node)

            Public Sub New(ByVal pos As UInt32, ByVal nodes As vector(Of typed_node))
                assert(Not nodes Is Nothing)
                Me.pos = pos
                Me.nodes = nodes
            End Sub

            Public Sub New(ByVal pos As UInt32, ByVal node As typed_node)
                Me.New(pos, vector.of(node))
            End Sub

            Public Sub New(ByVal pos As UInt32)
                Me.New(pos, New vector(Of typed_node)())
            End Sub

            Public Function node() As typed_node
                assert(nodes.size() = uint32_1)
                Return nodes(0)
            End Function
        End Class

        Public MustOverride Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of result)

        Protected Function create_node(ByVal v As vector(Of typed_word),
                                       ByVal type As UInt32,
                                       ByVal start As UInt32,
                                       ByVal [end] As UInt32) As typed_node
            Return New typed_node(v, type, type_name(type), start, [end])
        End Function

        Protected Function type_name(ByVal type As UInt32) As String
            If Not envs.utt.is_current Then
                Return c.type_name(type)
            End If

            Dim type_str As String = Nothing
            ' TODO: Fix the tests to avoid undefined type.
            If Not c.type_name(type, type_str) Then
                Return strcat("UNDEFINED_TYPE-", type)
            End If
            Return type_str
        End Function

        Protected Sub log_matching(ByVal v As vector(Of typed_word),
                                   ByVal start As UInt32,
                                   ByVal [end] As UInt32,
                                   ByVal matcher As Object)
            If syntaxer.debug_log Then
                Dim e As String = Nothing
                If [end] = v.size() Then
                    e = "[end]"
                Else
                    e = Convert.ToString(v([end]))
                End If
                raise_error(error_type.information, "Match token ", v(start), " to ", e, " with ", matcher)
            End If
        End Sub

        Protected Sub log_unmatched(ByVal v As vector(Of typed_word),
                                    ByVal start As UInt32,
                                    ByVal matcher As Object)
            If syntaxer.detailed_debug_log Then
                Dim start_word As String = Nothing
                If v.size() <= start Then
                    start_word = "end-of-typed-word"
                Else
                    start_word = Convert.ToString(v(start))
                End If
                raise_error(error_type.user, "Failed to match token ", start_word, " when matching with ", matcher)
            End If
        End Sub

        Protected Sub log_end_of_tokens(ByVal v As vector(Of typed_word),
                                        ByVal start As UInt32,
                                        ByVal matcher As Object)
            assert(start >= v.size())
            If syntaxer.debug_log Then
                raise_error(error_type.user, "End of tokens ", v.back(), "@", start, " when matching with ", matcher)
            End If
        End Sub

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of matching)(obj, False))
        End Function

        Public MustOverride Function CompareTo(ByVal other As matching) As Int32 _
                                              Implements IComparable(Of matching).CompareTo
    End Class

    Public NotInheritable Class matching_creator
        Private Sub New()
        End Sub

        Public Shared Function create(ByVal c As syntax_collection) As matching
            Return New empty_matching(c)
        End Function

        Public Shared Function create(ByVal c As syntax_collection, ByVal m As UInt32) As matching
            Return New single_matching(c, m)
        End Function

        Public Shared Function create(ByVal c As syntax_collection, ByVal ms() As UInt32) As matching
            If isemptyarray(ms) Then
                Return create(c)
            End If
            Return New matching_group(c, create_matchings(c, ms))
        End Function

        Public Shared Function create(ByVal c As syntax_collection,
                                      ByVal m1 As UInt32,
                                      ByVal m2 As UInt32,
                                      ByVal ParamArray ms() As UInt32) As matching
            Dim m() As UInt32 = Nothing
            ReDim m(2 + array_size_i(ms) - 1)
            m(0) = m1
            m(1) = m2
            If Not isemptyarray(ms) Then
                For i As Int32 = 0 To array_size_i(ms) - 1
                    m(i + 2) = ms(i)
                Next
            End If
            Return create(c, m)
        End Function

        Private Shared Function create_matchings(Of T)(ByVal c As syntax_collection,
                                                       ByVal ms() As T,
                                                       ByVal ctor As Func(Of syntax_collection, T, matching)) As matching()
            assert(Not ctor Is Nothing)
            If isemptyarray(ms) Then
                Return Nothing
            End If
            Dim m() As matching = Nothing
            ReDim m(array_size_i(ms) - 1)
            For i As Int32 = 0 To array_size_i(ms) - 1
                m(i) = ctor(c, ms(i))
            Next
            Return m
        End Function

        Public Shared Function create_matchings(ByVal c As syntax_collection,
                                                ByVal ParamArray ms() As UInt32) As matching()
            Return create_matchings(c, ms, AddressOf create)
        End Function

        Public Shared Function create_matchings(ByVal c As syntax_collection,
                                                ByVal ParamArray ms()() As UInt32) As matching()
            Return create_matchings(c, ms, AddressOf create)
        End Function

        Private Shared Function create_matching(ByVal s As String,
                                                ByVal collection As syntax_collection,
                                                ByRef o As matching) As Boolean
            assert(Not collection Is Nothing)
            Dim it As map(Of String, UInt32).iterator = Nothing
            assert(Not s Is Nothing)
            s = s.Trim()
            If Not characters.valid_type_str(s) Then
                Return False
            End If
            Dim i As UInt32 = 0
            If collection.token_type(s, i) Then
                o = matching_creator.create(collection, i)
            Else
                i = collection.define(s)
                o = New matching_delegate(collection, i)
            End If
            assert(Not o Is Nothing)
            Return True
        End Function

        Private Shared Sub consume_space_chars(ByVal i As String, ByRef pos As UInt32)
            While pos < strlen(i) AndAlso characters.matching_separators.Contains(i(CInt(pos)))
                pos += uint32_1
            End While
        End Sub

        Private Shared Function create_without_space_chars(ByVal i As String,
                                                           ByVal collection As syntax_collection,
                                                           ByRef pos As UInt32,
                                                           ByRef o As matching) As Boolean
            If strlen(i) <= pos Then
                Return False
            End If
            assert(Not characters.matching_separators.Contains(i(CInt(pos))))
            If i(CInt(pos)) = characters.matching_group_start Then
                Dim e As Int32 = 0
                e = strindexof(i, characters.matching_group_end, pos, uint32_1)
                If e = npos Then
                    Return False
                End If
                Dim ss As vector(Of String) = Nothing
                If strsplit(strmid(i, pos + uint32_1, CUInt(e) - pos - uint32_1),
                            characters.matching_group_separators,
                            default_strings,
                            ss,
                            False,
                            True) AndAlso
                   Not ss.null_or_empty() Then
                    Dim ms() As matching = Nothing
                    ReDim ms(CInt(ss.size() - uint32_1))
                    For j As UInt32 = 0 To ss.size() - uint32_1
                        If Not create_matching(ss(j), collection, ms(CInt(j))) Then
                            Return False
                        End If
                    Next
                    o = New matching_group(collection, ms)
                    pos = CUInt(e + 1)
                    While pos < strlen(i)
                        If i(CInt(pos)) = characters.optional_matching Then
                            o = New optional_matching_group(collection, o)
                            pos += uint32_1
                        ElseIf i(CInt(pos)) = characters.any_matching Then
                            o = New any_matching_group(collection, o)
                            pos += uint32_1
                        ElseIf i(CInt(pos)) = characters.multi_matching Then
                            o = New multi_matching_group(collection, o)
                            pos += uint32_1
                        Else
                            Exit While
                        End If
                    End While
                    Return True
                End If
                Return False
            Else
                Dim e As Int32 = 0
                e = i.IndexOfAny(characters.matching_separators_array, CInt(pos))
                If e = npos Then
                    e = i.Length()
                End If
                If create_matching(strmid(i, pos, CUInt(e) - pos), collection, o) Then
                    pos = CUInt(e)
                    Return True
                End If
                Return False
            End If
        End Function

        Public Shared Function create(ByVal i As String,
                                      ByVal collection As syntax_collection,
                                      ByRef pos As UInt32,
                                      ByRef o As matching) As Boolean
            consume_space_chars(i, pos)
            If create_without_space_chars(i, collection, pos, o) Then
                consume_space_chars(i, pos)
                Return True
            End If
            Return False
        End Function
    End Class
End Class
