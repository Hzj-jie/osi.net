
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class syntax_collection
        Implements IComparable, IComparable(Of syntax_collection)

        Private ReadOnly token_str_type As map(Of String, UInt32)
        Private ReadOnly syntax_str_type As map(Of String, UInt32)
        Private ReadOnly v As vector(Of syntax)
        Private next_type As UInt32

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
                    assert(define(token_str_type, token_str_types(i)))
                Next
            End If
            If Not isemptyarray(syntax_str_types) Then
                For i As UInt32 = 0 To array_size(syntax_str_types) - uint32_1
                    assert(define(syntax_str_type, syntax_str_types(i)))
                Next
            End If
        End Sub

        Public Sub New(ByVal token_str_type As map(Of String, UInt32))
            Me.New(True)
            assert(Not token_str_type Is Nothing)
            copy(Me.token_str_type, token_str_type)
            next_type = find_next_type()
        End Sub

        Private Function find_next_type() As UInt32
            assert(Not token_str_type Is Nothing)
            Dim max As UInt32 = 0
            Dim it As map(Of String, UInt32).iterator = Nothing
            it = token_str_type.begin()
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
            v.clear()
            next_type = find_next_type()
        End Sub

        Private Function define(ByVal m As map(Of String, UInt32),
                                ByVal name As String,
                                Optional ByRef o As UInt32 = Nothing) As Boolean
            assert(Not m Is Nothing)
            If characters.valid_type_str(name) Then
                Dim it As map(Of String, UInt32).iterator = Nothing
                it = m.find(name)
                If it = m.end() Then
                    o = next_type
                    m(name) = o
                    next_type += 1
                Else
                    o = (+it).second
                End If
                Return True
            Else
                Return False
            End If
        End Function

        Public Function define(ByVal name As String, ByRef o As UInt32) As Boolean
            assert(Not token_type(name, uint32_0))
            Return define(syntax_str_type, name, o)
        End Function

        Public Function define(ByVal name As String) As UInt32
            Dim o As UInt32 = 0
            assert(define(name, o))
            Return o
        End Function

        Private Shared Function str_type(ByVal m As map(Of String, UInt32),
                                         ByVal name As String,
                                         ByRef o As UInt32) As Boolean
            assert(Not m Is Nothing)
            If characters.valid_type_str(name) Then
                Dim it As map(Of String, UInt32).iterator = Nothing
                it = m.find(name)
                If it = m.end() Then
                    Return False
                Else
                    o = (+it).second
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Public Function token_type(ByVal name As String, ByRef o As UInt32) As Boolean
            Return str_type(token_str_type, name, o)
        End Function

        Public Function syntax_type(ByVal name As String, ByRef o As UInt32) As Boolean
            Return str_type(syntax_str_type, name, o)
        End Function

        Public Function type_id(ByVal name As String, ByRef o As UInt32) As Boolean
            Return token_type(name, o) OrElse syntax_type(name, o)
        End Function

        Public Function type_id(ByVal name As String) As UInt32
            Dim o As UInt32 = 0
            assert(type_id(name, o))
            Return o
        End Function

        Public Function [set](ByVal s As syntax) As Boolean
            If s Is Nothing Then
                Return False
            ElseIf v.size() <= s.type Then
                v.resize(s.type + uint32_1)
            End If
            If v(s.type) Is Nothing Then
                v(s.type) = s
                Return True
            Else
                Return False
            End If
        End Function

        Public Function [get](ByVal type As UInt32, ByRef o As syntax) As Boolean
            If type < v.size() AndAlso Not v(type) Is Nothing Then
                o = v(type)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function complete() As Boolean
            Dim it As map(Of String, UInt32).iterator = Nothing
            it = syntax_str_type.begin()
            Dim s As [set](Of UInt32) = Nothing
            s = New [set](Of UInt32)()
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
            Else
                Return c
            End If
        End Function
    End Class
End Class
