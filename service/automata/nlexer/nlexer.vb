
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata.syntaxer

Partial Public NotInheritable Class nlexer
    Private ReadOnly rs As vector(Of pair(Of String, rule))

    Public Sub New(ByVal ParamArray rs() As pair(Of String, rule))
        Me.New(vector.of(rs))
    End Sub

    Private Sub New(ByVal rs As vector(Of pair(Of String, rule)))
        assert(Not rs Is Nothing)
        assert(Not rs.empty())
        Me.rs = rs
    End Sub

    Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of result)
        Dim j As UInt32 = 0
        Dim max As [optional](Of result) = Nothing
        max = [optional].empty(Of result)()
        While j < rs.size()
            Dim r As [optional](Of UInt32) = Nothing
            r = rs(j).second.match(i, pos)
            If (Not r.empty()) AndAlso ((Not max) OrElse (+max).end < (+r)) Then
                max = [optional].of(New result(pos, +r, rs(j).first, j))
            End If
            j += uint32_1
        End While
        Return max
    End Function

    Public Function match(ByVal i As String,
                          ByRef o As vector(Of result),
                          ByVal ParamArray ignore_types() As String) As Boolean
        o.renew()
        Dim pos As UInt32 = 0
        While pos < strlen(i)
            Dim r As [optional](Of result) = Nothing
            r = match(i, pos)
            If Not r Then
                raise_error(error_type.user, no_match_str(i, pos))
                Return False
            End If
            If Not ignore_types.has((+r).name) Then
                o.emplace_back(+r)
            End If
            pos = (+r).end
        End While
        Return True
    End Function

    Public Function match(ByVal i As String,
                          ByRef o As vector(Of typed_word),
                          ByVal ParamArray ignore_types() As String) As Boolean
        Dim r As vector(Of nlexer.result) = Nothing
        If Not match(i, r, ignore_types) Then
            Return False
        End If
        o = nlexer.result.typed_words(i, r)
        Return True
    End Function

    Public Function result_of(ByVal start As UInt32, ByVal [end] As UInt32, ByVal name As String) As result
        Dim j As UInt32 = 0
        While j < rs.size()
            If strsame(name, rs(j).first) Then
                Return New result(start, [end], name, j)
            End If
            j += uint32_1
        End While
        assert(False)
        Return Nothing
    End Function

    Private Shared Function no_match_str(ByVal i As String, ByVal pos As UInt32) As String
        Static half_short_str_len As UInt32 = 10
        Dim short_str As String = Nothing
        short_str = strmid(i, If(pos > half_short_str_len, pos - half_short_str_len, uint32_0), half_short_str_len << 1)
        Return strcat("No match in ", i, " at pos ", pos, " -> ", short_str, " >> [" + i.char_at(pos), "]")
    End Function

    Public Function match(ByVal ParamArray s() As String) As [optional](Of vector(Of result))
        Static empty_array() As String = {}
        Return match(s, empty_array)
    End Function

    Public Function match(ByVal s() As String,
                          ByVal ParamArray ignore_types() As String) As [optional](Of vector(Of result))
        Dim v As vector(Of result) = Nothing
        If match(s.strjoin(character.newline), v, ignore_types) Then
            Return [optional].of(v)
        End If
        Return [optional].empty(Of vector(Of result))()
    End Function

    Public Shared Function [of](ByVal parse As Func(Of parser, Boolean), ByRef o As nlexer) As Boolean
        assert(Not parse Is Nothing)
        Dim p As parser = Nothing
        p = New parser()
        If parse(p) Then
            o = p.export()
            Return True
        End If
        Return False
    End Function

    Public Shared Function [of](ByVal i As String, ByRef o As nlexer) As Boolean
        Return [of](Function(ByVal p As parser) As Boolean
                        assert(Not p Is Nothing)
                        Return p.parse_content(i)
                    End Function,
                    o)
    End Function

    Public Shared Function [of](ByVal ls() As String, ByRef o As nlexer) As Boolean
        Return [of](Function(ByVal p As parser) As Boolean
                        assert(Not p Is Nothing)
                        Return p.parse(ls)
                    End Function,
                    o)
    End Function

    Public Shared Function of_file(ByVal file As String, ByRef o As nlexer) As Boolean
        Return [of](Function(ByVal p As parser) As Boolean
                        assert(Not p Is Nothing)
                        Return p.parse_file(file)
                    End Function,
                    o)
    End Function

    Public Shared Function [of](ByVal ParamArray ls() As String) As [optional](Of nlexer)
        Dim o As nlexer = Nothing
        If [of](ls, o) Then
            Return [optional].of(o)
        End If
        Return [optional].empty(Of nlexer)()
    End Function

    Public Function str_type_mapping() As syntax_collection
        Return New syntax_collection(map.emplace_index(+rs.map(Function(ByVal i As pair(Of String, rule)) As String
                                                                   assert(Not i Is Nothing)
                                                                   Return i.first
                                                               End Function)))
    End Function
End Class
