
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public MustInherit Class matching
        Implements IComparable, IComparable(Of matching)

        Public MustOverride Function match(ByVal v As vector(Of typed_word),
                                           ByRef p As UInt32,
                                           ByVal parent As typed_node) As Boolean

        Public Function match(ByVal v As vector(Of typed_word), ByRef p As UInt32) As Boolean
            Return match(v, p, Nothing)
        End Function

        Protected Shared Function add_subnode(ByVal v As vector(Of typed_word),
                                              ByVal parent As typed_node,
                                              ByVal type As UInt32,
                                              ByVal start As UInt32,
                                              ByVal [end] As UInt32) As typed_node
            If parent Is Nothing Then
                Return Nothing
            Else
                Dim r As typed_node = Nothing
                r = New typed_node(v, type, start, [end])
                parent.subnodes.emplace_back(r)
                Return r
            End If
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of matching)(obj, False))
        End Function

        Public MustOverride Function CompareTo(ByVal other As matching) As Int32 _
                                              Implements IComparable(Of matching).CompareTo
    End Class

    Public Class matching_creator
        Private Sub New()
        End Sub

        Public Shared Function create() As matching
            Return New empty_matching()
        End Function

        Public Shared Function create(ByVal m As UInt32) As matching
            Return New single_matching(m)
        End Function

        Public Shared Function create(ByVal ms() As UInt32) As matching
            If isemptyarray(ms) Then
                Return create()
            Else
                Return New matching_group(create_matchings(ms))
            End If
        End Function

        Public Shared Function create(ByVal m1 As UInt32,
                                      ByVal m2 As UInt32,
                                      ByVal ParamArray ms() As UInt32) As matching
            Dim m() As UInt32 = Nothing
            ReDim m(uint32_2 + array_size(ms) - uint32_1)
            m(0) = m1
            m(1) = m2
            If Not isemptyarray(ms) Then
                For i As UInt32 = 0 To array_size(ms) - uint32_1
                    m(i + uint32_2) = ms(i)
                Next
            End If
            Return create(m)
        End Function

        Private Shared Function create_matchings(Of T)(ByVal ms() As T, ByVal c As Func(Of T, matching)) As matching()
            assert(Not c Is Nothing)
            If isemptyarray(ms) Then
                Return Nothing
            Else
                Dim m() As matching = Nothing
                ReDim m(array_size(ms) - uint32_1)
                For i As UInt32 = 0 To array_size(ms) - uint32_1
                    m(i) = c(ms(i))
                Next
                Return m
            End If
        End Function

        Public Shared Function create_matchings(ByVal ParamArray ms() As UInt32) As matching()
            Return create_matchings(ms, AddressOf create)
        End Function

        Public Shared Function create_matchings(ByVal ParamArray ms()() As UInt32) As matching()
            Return create_matchings(ms, AddressOf create)
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
                o = matching_creator.create(i)
            Else
                i = collection.define(s)
                o = New matching_delegate(collection, i)
            End If
            assert(Not o Is Nothing)
            Return True
        End Function

        Private Shared Sub consume_space_chars(ByVal i As String, ByRef pos As UInt32)
            While pos < strlen(i) AndAlso characters.matching_separators.Contains(i(pos))
                pos += uint32_1
            End While
        End Sub

        Private Shared Function create_without_space_chars(ByVal i As String,
                                                           ByVal collection As syntax_collection,
                                                           ByRef pos As UInt32,
                                                           ByRef o As matching) As Boolean
            If strlen(i) <= pos Then
                Return False
            Else
                assert(Not characters.matching_separators.Contains(i(pos)))
                If i(pos) = characters.matching_group_start Then
                    Dim e As Int32 = 0
                    e = strindexof(i, characters.matching_group_end, pos, uint32_1)
                    If e = npos Then
                        Return False
                    Else
                        Dim ss As vector(Of String) = Nothing
                        If strsplit(strmid(i, pos + uint32_1, e - pos - uint32_1),
                                    characters.matching_group_separators,
                                    default_strings,
                                    ss,
                                    False,
                                    True) AndAlso
                           Not ss.null_or_empty() Then
                            Dim ms() As matching = Nothing
                            ReDim ms(ss.size() - uint32_1)
                            For j As UInt32 = 0 To ss.size() - uint32_1
                                If Not create_matching(ss(j), collection, ms(j)) Then
                                    Return False
                                End If
                            Next
                            o = New matching_group(ms)
                            pos = e + 1
                            While pos < strlen(i)
                                If i(pos) = characters.optional_matching Then
                                    o = New optional_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.any_matching Then
                                    o = New any_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.multi_matching Then
                                    o = New multi_matching_group(o)
                                    pos += uint32_1
                                Else
                                    Exit While
                                End If
                            End While
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Else
                    Dim e As Int32 = 0
                    e = i.IndexOfAny(characters.matching_separators_array, pos)
                    If e = npos Then
                        e = i.Length()
                    End If
                    If create_matching(strmid(i, pos, e - pos), collection, o) Then
                        pos = e
                        Return True
                    Else
                        Return False
                    End If
                End If
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
            Else
                Return False
            End If
        End Function
    End Class
End Class
