
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports characters = osi.service.automata.syntaxer.characters
Imports envs = osi.root.envs
Imports syntax = osi.service.automata.syntaxer.syntax

Public NotInheritable Class syntax_collection
    Implements IComparable, IComparable(Of syntax_collection), ICloneable, ICloneable(Of syntax_collection)

    Private ReadOnly token_str_type As unordered_map(Of String, UInt32)
    Private ReadOnly str_token_type As const_array(Of String)
    Private ReadOnly syntax_str_type As unordered_map(Of String, UInt32)
    Private ReadOnly str_syntax_type As vector(Of String)
    Private ReadOnly v As vector(Of syntax)

    <copy_constructor>
    Private Sub New(ByVal token_str_type As unordered_map(Of String, UInt32),
                    ByVal str_token_type As const_array(Of String),
                    ByVal syntax_str_type As unordered_map(Of String, UInt32),
                    ByVal str_syntax_type As vector(Of String),
                    ByVal v As vector(Of syntax))
        assert(Not token_str_type Is Nothing)
        assert(Not str_token_type Is Nothing)
        assert(Not syntax_str_type Is Nothing)
        assert(Not str_syntax_type Is Nothing)
        assert(Not v Is Nothing)
        Me.token_str_type = token_str_type
        Me.str_token_type = str_token_type
        Me.syntax_str_type = syntax_str_type
        Me.str_syntax_type = str_syntax_type
        Me.v = v
    End Sub

    ' test only
    Public Sub New()
        Me.New(New unordered_map(Of String, UInt32)())
    End Sub

    ' test only
    Public Sub New(ByVal token_str_types() As String, ByVal syntax_str_types() As String)
        token_str_type = New unordered_map(Of String, UInt32)()
        syntax_str_type = New unordered_map(Of String, UInt32)()
        v = New vector(Of syntax)()
        If Not token_str_types.isemptyarray() Then
            For i As UInt32 = 0 To array_size(token_str_types) - uint32_1
                assert(characters.valid_type_str(token_str_types(CInt(i))))
                assert(token_str_type.emplace(token_str_types(CInt(i)), i).second())
            Next
        End If
        str_token_type = const_array.of(token_str_types)
        If Not syntax_str_types.isemptyarray() Then
            For i As UInt32 = 0 To array_size(syntax_str_types) - uint32_1
                assert(characters.valid_type_str(syntax_str_types(CInt(i))))
                assert(syntax_str_type.emplace(syntax_str_types(CInt(i)), i).second())
            Next
        End If
        str_syntax_type = vector.emplace_of(syntax_str_types)
    End Sub

    Public Sub New(ByVal token_str_type As unordered_map(Of String, UInt32))
        assert(Not token_str_type Is Nothing)
        Me.token_str_type = token_str_type.CloneT()
        Me.syntax_str_type = New unordered_map(Of String, UInt32)()
        Me.str_token_type = const_array.of(token_str_type.stream().
                                                          sort(first_const_pair(Of String, UInt32).second_comparer).
                                                          map(first_const_pair(Of String, UInt32).first_getter).
                                                          to_array())
        Me.str_syntax_type = New vector(Of String)()
        Me.v = New vector(Of syntax)()

        If syntaxer.dump_rules Then
            Dim it As unordered_map(Of String, UInt32).iterator = token_str_type.begin()
            While it <> token_str_type.end()
                raise_error(error_type.user, "Define token type ", (+it).first, ": ", (+it).second)
                it += 1
            End While
        End If
    End Sub

    Private Function next_type() As UInt32
        Return str_token_type.size() + str_syntax_type.size()
    End Function

    Public Sub clear()
        syntax_str_type.clear()
        str_syntax_type.clear()
        v.clear()
    End Sub

    Private Function define(ByVal m As unordered_map(Of String, UInt32),
                            ByVal name As String,
                            Optional ByRef o As UInt32 = Nothing) As Boolean
        assert(Not m Is Nothing)
        If Not characters.valid_type_str(name) Then
            Return False
        End If
        Dim it As unordered_map(Of String, UInt32).iterator = m.find(name)
        If it = m.end() Then
            o = next_type()
            m.emplace(name, o)
        Else
            o = (+it).second
        End If
        Return True
    End Function

    Public Function define(ByVal name As String, ByRef o As UInt32) As Boolean
        assert(Not token_type(name, uint32_0))
        If Not define(syntax_str_type, name, o) Then
            Return False
        End If
        str_syntax_type.emplace_back(name)
        If syntaxer.dump_rules AndAlso o = next_type - uint32_1 Then
            raise_error(error_type.user, "Define syntax type ", name, ": ", o)
        End If
        Return True
    End Function

    Public Function define(ByVal name As String) As UInt32
        Dim o As UInt32 = 0
        assert(define(name, o))
        Return o
    End Function

    Private Shared Function str_type(ByVal m As unordered_map(Of String, UInt32),
                                     ByVal name As String,
                                     ByRef o As UInt32) As Boolean
        assert(Not m Is Nothing)
        If Not characters.valid_type_str(name) Then
            Return False
        End If
        Dim it As unordered_map(Of String, UInt32).iterator = m.find(name)
        If it = m.end() Then
            Return False
        End If
        o = (+it).second
        Return True
    End Function

    Public Function token_type(ByVal name As String, ByRef o As UInt32) As Boolean
        Return str_type(token_str_type, name, o)
    End Function

    Public Function syntax_type(ByVal name As String, ByRef o As UInt32) As Boolean
        Return str_type(syntax_str_type, name, o)
    End Function

    Public Function type_token(ByVal id As UInt32, ByRef o As String) As Boolean
        If id < str_token_type.size() Then
            o = str_token_type(id)
            Return True
        End If
        Return False
    End Function

    Public Function type_syntax(ByVal id As UInt32, ByRef o As String) As Boolean
        If id >= str_token_type.size() AndAlso id < next_type() Then
            o = str_syntax_type(id - str_token_type.size())
            Return True
        End If
        Return False
    End Function

    Public Function type_id(ByVal name As String, ByRef o As UInt32) As Boolean
        Return token_type(name, o) OrElse syntax_type(name, o)
    End Function

    Public Function type_id(ByVal name As String) As UInt32
        Dim o As UInt32 = 0
        assert(type_id(name, o))
        Return o
    End Function

    Public Function type_name(ByVal id As UInt32, ByRef o As String) As Boolean
        Return type_token(id, o) OrElse type_syntax(id, o)
    End Function

    Public Function type_name(ByVal id As UInt32) As String
        If Not envs.utt.is_current Then
            Dim o As String = Nothing
            assert(type_name(id, o))
            Return o
        End If

        Dim type_str As String = Nothing
        ' TODO: Fix the tests to avoid undefined type.
        If Not type_name(id, type_str) Then
            Return strcat("UNDEFINED_TYPE-", id)
        End If
        Return type_str
    End Function

    Public Function [set](ByVal s As syntax) As Boolean
        If s Is Nothing Then
            Return False
        End If
        If v.size() <= s.type Then
            v.resize(s.type + uint32_1)
        End If
        If v(s.type) Is Nothing Then
            v(s.type) = s
            Return True
        End If
        Return False
    End Function

    Public Function [get](ByVal type As UInt32, ByRef o As syntax) As Boolean
        If type < v.size() AndAlso Not v(type) Is Nothing Then
            o = v(type)
            Return True
        End If
        Return False
    End Function

    Public Function [get](ByVal types() As UInt32, ByRef o As vector(Of syntax)) As Boolean
        o.renew()
        For i As Int32 = 0 To array_size_i(types) - 1
            Dim s As syntax = Nothing
            If Not [get](types(i), s) Then
                Return False
            End If
            o.emplace_back(s)
        Next
        Return True
    End Function

    Public Function [get](ByVal types() As UInt32) As vector(Of syntax)
        Dim o As vector(Of syntax) = Nothing
        assert([get](types, o))
        Return o
    End Function

    Public Function complete() As Boolean
        Dim it As unordered_map(Of String, UInt32).iterator = syntax_str_type.begin()
        Dim s As New [set](Of UInt32)()
        While it <> syntax_str_type.end()
            assert(s.insert((+it).second).second)
            If Not [get]((+it).second, Nothing) Then
                raise_error(error_type.user, "type ", (+it).first, " has not been defined.")
                Return False
            End If
            it += 1
        End While

        If Not v.empty() Then
            ' define should be called before set
            For i As UInt32 = 0 To v.size() - uint32_1
                If [get](i, Nothing) Then
                    assert(s.find(i) <> s.end())
                End If
            Next
        End If
        Return True
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of syntax_collection)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As syntax_collection) As Int32 _
                             Implements IComparable(Of syntax_collection).CompareTo
        Dim c As Int32 = object_compare(Me, other)
        If c = object_compare_undetermined Then
            Return compare(Me.v, other.v)
        End If
        Return c
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As syntax_collection Implements ICloneable(Of syntax_collection).Clone
        Return copy_constructor(Of syntax_collection).copy_from(token_str_type,
                                                                str_token_type,
                                                                syntax_str_type,
                                                                str_syntax_type,
                                                                v)
    End Function
End Class
