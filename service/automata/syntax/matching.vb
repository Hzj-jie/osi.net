
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class syntaxer
    Public MustInherit Class matching
        Implements IComparable, IComparable(Of matching)

        Private Interface disable_cycle_dependency_check_protector
        End Interface

        Private Shared disable_cycle_dependency_check As argument(Of Boolean)

        <ThreadStatic> Private Shared s As map(Of UInt32, UInt32)
        Protected ReadOnly c As syntax_collection

        Protected Sub New(ByVal c As syntax_collection)
            assert(Not c Is Nothing)
            Me.c = c
        End Sub

        Public Shared Function disable_cycle_dependency_check_in_thread() As IDisposable
            Return thread_static_argument_default(Of Boolean, disable_cycle_dependency_check_protector).
                       scoped_register(True)
        End Function

        Public Structure result
            Public Structure suc_t
                Public ReadOnly pos As UInt32
                Public ReadOnly nodes As vector(Of typed_node)

                Public Sub New(ByVal pos As UInt32, ByVal nodes As vector(Of typed_node))
                    assert(Not nodes Is Nothing)
                    Me.pos = pos
                    Me.nodes = nodes
                End Sub

                Public Function null() As Boolean
                    Return nodes Is Nothing
                End Function

                Public Shared Operator Or(ByVal this As suc_t, ByVal that As suc_t) As suc_t
                    assert(Not this.null())
                    assert(Not that.null())
                    If this.pos >= that.pos Then
                        Return this
                    End If
                    Return that
                End Operator
            End Structure

            ' Not null
            Public Structure fal_t
                Public ReadOnly pos As UInt32

                Public Sub New(ByVal pos As UInt32)
                    Me.pos = pos
                End Sub

                Public Shared Operator Or(ByVal this As fal_t, ByVal that As fal_t) As fal_t
                    If this.pos >= that.pos Then
                        Return this
                    End If
                    Return that
                End Operator
            End Structure

            Public ReadOnly suc As suc_t
            Public ReadOnly fal As fal_t

            Private Sub New(ByVal suc As suc_t, ByVal fal As fal_t)
                Me.suc = suc
                Me.fal = fal
            End Sub

            Public Function succeeded() As Boolean
                Return Not suc.null()
            End Function

            Public Function failed() As Boolean
                Return Not succeeded()
            End Function

            Public Shared Function failure(ByVal p As UInt32) As result
                Return New result(Nothing, New fal_t(p))
            End Function

            Public Shared Function success(ByVal p As UInt32, ByVal nodes As vector(Of typed_node)) As result
                Return New result(New suc_t(p, nodes), New fal_t(0))
            End Function

            Public Shared Function success(ByVal p As UInt32, ByVal node As typed_node) As result
                Return success(p, vector.of(node))
            End Function

            Public Shared Function success(ByVal p As UInt32) As result
                Return success(p, New vector(Of typed_node)())
            End Function

            Public Shared Operator Or(ByVal this As result, ByVal that As result) As result
                If this.suc.null() AndAlso that.suc.null() Then
                    Return New result(Nothing, this.fal Or that.fal)
                End If
                If this.suc.null() Then
                    Return New result(that.suc, this.fal Or that.fal)
                End If
                If that.suc.null() Then
                    Return New result(this.suc, this.fal Or that.fal)
                End If
                Return New result(this.suc Or that.suc, this.fal Or that.fal)
            End Operator
        End Structure

        Public MustOverride Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As result

        Protected Function create_node(ByVal v As vector(Of typed_word),
                                       ByVal type As UInt32,
                                       ByVal start As UInt32,
                                       ByVal [end] As UInt32) As typed_node
            Return New typed_node(v, type, c.type_name(type), start, [end])
        End Function

        Protected Function disallow_cycle_dependency(
                               ByVal type As UInt32,
                               ByVal pos As UInt32,
                               ByVal f As Func(Of result)) As result
            If disable_cycle_dependency_check Or
               thread_static_argument_default(Of disable_cycle_dependency_check_protector).of(False) Then
                Return f()
            End If
            assert(Not f Is Nothing)
            If s Is Nothing Then
                s = New map(Of UInt32, UInt32)()
            End If
            Dim previous As [optional](Of UInt32) = s.find_opt(type)
            If previous Then
                assert((+previous) <= pos)
                If (+previous) = pos Then
                    raise_error(error_type.user, "Cycle dependency found at ", c.type_name(type))
                    Return result.failure(pos)
                End If
            End If
            s(type) = pos

            Using defer.to(Sub()
                               assert(s(type) = pos)
                               If previous Then
                                   s(type) = (+previous)
                               Else
                                   assert(s.erase(type))
                               End If
                           End Sub)
                Return f()
            End Using
        End Function

        Protected Sub log_matching(ByVal v As vector(Of typed_word),
                                   ByVal start As UInt32,
                                   ByVal [end] As UInt32,
                                   ByVal matcher As Object)
            If Not syntaxer.debug_log Then
                Return
            End If
            Dim e As String = Nothing
            If [end] = v.size() Then
                e = "[end]"
            Else
                e = Convert.ToString(v([end]))
            End If
            raise_error(error_type.information, "Match token ", v(start), " to ", e, " with ", matcher)
        End Sub

        Protected Sub log_unmatched(ByVal v As vector(Of typed_word),
                                    ByVal start As UInt32,
                                    ByVal matcher As Object)
            If Not syntaxer.detailed_debug_log Then
                Return
            End If
            Dim start_word As String = Nothing
            If v.size() <= start Then
                start_word = "end-of-typed-word"
            Else
                start_word = Convert.ToString(v(start))
            End If
            raise_error(error_type.user, "Failed to match token ", start_word, " when matching with ", matcher)
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

        Public Shared Function create(ByVal c As syntax_collection, ByVal ParamArray ms() As UInt32) As matching
            If isemptyarray(ms) Then
                Return create(c)
            End If
            Return New matching_group(c, create_matchings(c, ms))
        End Function

        Private Shared Function create_matchings(Of T) _
                                                (ByVal c As syntax_collection,
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
            Dim e As Int32 = 0
            If i(CInt(pos)) = characters.matching_group_start Then
                e = strindexof(i, characters.matching_group_end, pos, uint32_1)
                If e = npos Then
                    Return False
                End If
                Dim ss As vector(Of String) = Nothing
                If Not strsplit(strmid(i, pos + uint32_1, CUInt(e) - pos - uint32_1),
                                characters.matching_group_separators,
                                default_strings,
                                ss,
                                False,
                                True) OrElse
                   ss.null_or_empty() Then
                    Return False
                End If
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
            e = i.IndexOfAny(characters.matching_separators_array, CInt(pos))
            If e = npos Then
                e = i.Length()
            End If
            If Not create_matching(strmid(i, pos, CUInt(e) - pos), collection, o) Then
                Return False
            End If
            pos = CUInt(e)
            Return True
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
