
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

    Private ReadOnly token_str_type As map(Of String, UInt32)
    Private ReadOnly str_token_type As map(Of UInt32, String)
    Private ReadOnly syntax_str_type As map(Of String, UInt32)
    Private ReadOnly str_syntax_type As map(Of UInt32, String)
    Private ReadOnly v As vector(Of syntax)
    Private next_type As UInt32

    <copy_constructor>
    Private Sub New(ByVal token_str_type As map(Of String, UInt32),
                    ByVal str_token_type As map(Of UInt32, String),
                    ByVal syntax_str_type As map(Of String, UInt32),
                    ByVal str_syntax_type As map(Of UInt32, String),
                    ByVal v As vector(Of syntax),
                    ByVal next_type As UInt32)
        assert(token_str_type IsNot Nothing)
        assert(str_token_type IsNot Nothing)
        assert(syntax_str_type IsNot Nothing)
        assert(str_syntax_type IsNot Nothing)
        assert(v IsNot Nothing)
        Me.token_str_type = token_str_type
        Me.str_token_type = str_token_type
        Me.syntax_str_type = syntax_str_type
        Me.str_syntax_type = str_syntax_type
        Me.v = v
        Me.next_type = next_type
    End Sub

    Private Sub New(ByVal private_constructor As Boolean)
        syntax_str_type = New map(Of String, UInt32)()
        v = New vector(Of syntax)()
    End Sub

    ' test only
    Public Sub New()
        Me.New(New map(Of String, UInt32)())
    End Sub

    ' test only
    Public Sub New(ByVal token_str_types() As String, ByVal syntax_str_types() As String)
        Me.New(True)
        token_str_type = New map(Of String, UInt32)()
        next_type = 0
        If Not isemptyarray(token_str_types) Then
            For i As UInt32 = 0 To array_size(token_str_types) - uint32_1
                assert(define(token_str_type, token_str_types(CInt(i))))
            Next
        End If
        If Not isemptyarray(syntax_str_types) Then
            For i As UInt32 = 0 To array_size(syntax_str_types) - uint32_1
                assert(define(syntax_str_type, syntax_str_types(CInt(i))))
            Next
        End If

        str_token_type = token_str_type.emplace_reverse()
        str_syntax_type = syntax_str_type.emplace_reverse()
    End Sub

    Public Sub New(ByVal token_str_type As map(Of String, UInt32))
        Me.New(True)
        assert(token_str_type IsNot Nothing)
        copy(Me.token_str_type, token_str_type)
        next_type = find_next_type()

        str_token_type = token_str_type.emplace_reverse()
        str_syntax_type = syntax_str_type.emplace_reverse()

        If syntaxer.dump_rules Then
            Dim it As map(Of String, UInt32).iterator = Nothing
            it = token_str_type.begin()
            While it <> token_str_type.end()
                raise_error(error_type.user, "Define token type ", (+it).first, ": ", (+it).second)
                it += 1
            End While
        End If
    End Sub

    Private Function find_next_type() As UInt32
        assert(token_str_type IsNot Nothing)
        Dim max As UInt32 = 0
        Dim it As map(Of String, UInt32).iterator = token_str_type.begin()
        While it <> token_str_type.end()
            If (+it).second > max Then
                max = (+it).second
            End If
            it += 1
        End While
        assert(max < max_uint32)
        Return max + uint32_1
    End Function

    Public Sub clear()
        syntax_str_type.clear()
        str_syntax_type.clear()
        v.clear()
        next_type = find_next_type()
    End Sub

    Private Function define(ByVal m As map(Of String, UInt32),
                            ByVal name As String,
                            Optional ByRef o As UInt32 = Nothing) As Boolean
        assert(m IsNot Nothing)
        If Not characters.valid_type_str(name) Then
            Return False
        End If
        Dim it As map(Of String, UInt32).iterator = m.find(name)
        If it = m.end() Then
            o = next_type
            m.emplace(name, o)
            next_type += uint32_1
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
        str_syntax_type.emplace(o, name)
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

    Private Shared Function str_type(ByVal m As map(Of String, UInt32),
                                     ByVal name As String,
                                     ByRef o As UInt32) As Boolean
        assert(m IsNot Nothing)
        If Not characters.valid_type_str(name) Then
            Return False
        End If
        Dim it As map(Of String, UInt32).iterator = Nothing
        it = m.find(name)
        If it = m.end() Then
            Return False
        End If
        o = (+it).second
        Return True
    End Function

    Private Shared Function type_str(ByVal m As map(Of UInt32, String),
                                     ByVal id As UInt32,
                                     ByRef o As String) As Boolean
        assert(m IsNot Nothing)
        Dim it As map(Of UInt32, String).iterator = Nothing
        it = m.find(id)
        If it = m.end() Then
            Return False
        End If
        o = (+it).second
        assert(characters.valid_type_str(o))
        Return True
    End Function

    Public Function token_type(ByVal name As String, ByRef o As UInt32) As Boolean
        Return str_type(token_str_type, name, o)
    End Function

    Public Function syntax_type(ByVal name As String, ByRef o As UInt32) As Boolean
        Return str_type(syntax_str_type, name, o)
    End Function

    Public Function type_token(ByVal id As UInt32, ByRef o As String) As Boolean
        Return type_str(str_token_type, id, o)
    End Function

    Public Function type_syntax(ByVal id As UInt32, ByRef o As String) As Boolean
        Return type_str(str_syntax_type, id, o)
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
        If type < v.size() AndAlso v(type) IsNot Nothing Then
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
        Dim it As map(Of String, UInt32).iterator = syntax_str_type.begin()
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
        Dim c As Int32 = 0
        c = object_compare(Me, other)
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
                                                                v,
                                                                next_type)
    End Function
End Class
