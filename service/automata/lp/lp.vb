
Imports System.IO
Imports System.Reflection
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class lp(Of MAX_TYPE As _int64, RESULT_T)
    Private ReadOnly l As lexer
    Private ReadOnly p As parser(Of MAX_TYPE, RESULT_T)
    Private ReadOnly i As input

    Protected Sub New()
        l = New lexer()
        p = New parser(Of MAX_TYPE, RESULT_T)()
        i = New input()
    End Sub

    Private Function define_lexer(ByRef type_name_id As map(Of String, Int32)) As Boolean
        If i.use_default_separators() Then
            l.use_default_separators(i.ignore_separators())
        End If
        type_name_id = New map(Of String, Int32)()
        type_name_id(separator_type_name) = lexer.separator_type
        type_name_id(unknown_type_name) = lexer.unknown_type
        type_name_id(end_type_name) = lexer.end_type
        Dim it As map(Of String, pair(Of vector(Of String), Boolean)).iterator = Nothing
        Dim id As Int32 = 0
        it = i.types.begin()
        id = lexer.first_user_type
        While it <> i.types.end()
            Dim cid As Int32 = 0
            If strsame((+it).first, separator_type_name) Then
                cid = lexer.separator_type
            ElseIf strsame((+it).first, unknown_type_name) Then
                cid = lexer.unknown_type
            ElseIf strsame((+it).first, end_type_name) Then
                cid = lexer.end_type
            ElseIf type_name_id.find((+it).first) <> type_name_id.end() Then
                cid = type_name_id((+it).first)
            Else
                cid = id
            End If
            If Not l.define(+((+it).second.first), cid, (+it).second.second) Then
                Return False
            End If
            If cid = id Then
                type_name_id((+it).first) = id
                id += 1
            End If
            it += 1
        End While
        Return True
    End Function

    Private Shared Function allocate_status_name_id(ByVal status_name_id As map(Of String, UInt32),
                                                    ByVal name As String,
                                                    ByRef id As UInt32) As Boolean
        assert(Not status_name_id Is Nothing)
        assert(Not String.IsNullOrEmpty(name))
        assert(id >= parser(Of MAX_TYPE, RESULT_T).first_user_status)
        If Not strsame(name, start_status_name) AndAlso
           Not strsame(name, end_status_name) AndAlso
           status_name_id.find(name) = status_name_id.end() Then
            status_name_id(name) = id
            id += 1
        End If
        Return True
    End Function

    Private Function allocate_status_name_id(ByRef status_name_id As map(Of String, UInt32)) As Boolean
        status_name_id = New map(Of String, UInt32)
        status_name_id(start_status_name) = parser(Of MAX_TYPE, RESULT_T).start_status
        status_name_id(end_status_name) = parser(Of MAX_TYPE, result).end_status
        Dim it As map(Of String, map(Of String, pair(Of String, String))).iterator = Nothing
        Dim id As UInt32 = 0
        it = i.statuses.begin()
        id = parser(Of MAX_TYPE, RESULT_T).first_user_status
        While it <> i.statuses.end()
            If Not allocate_status_name_id(status_name_id, (+it).first, id) Then
                Return False
            End If
            Dim it2 As map(Of String, pair(Of String, String)).iterator = Nothing
            it2 = (+it).second.begin()
            While it2 <> (+it).second.end()
                If Not allocate_status_name_id(status_name_id, (+it2).second.first, id) Then
                    Return False
                End If
                it2 += 1
            End While
            it += 1
        End While
        Return True
    End Function

    Private Shared Function retrieve_map(Of T)(ByVal m As map(Of String, T),
                                               ByVal name As String,
                                               ByRef o As T) As Boolean
        assert(Not m Is Nothing)
        assert(Not String.IsNullOrEmpty(name))
        Dim it As map(Of String, T).iterator = Nothing
        it = m.find(name)
        If it = m.end() Then
            Return False
        Else
            o = (+it).second
            Return True
        End If
    End Function

    Private Shared Function retrieve_type_id(ByVal type_name_id As map(Of String, Int32),
                                             ByVal name As String,
                                             ByRef id As Int32) As Boolean
        Return retrieve_map(type_name_id, name, id)
    End Function

    Private Shared Function retrieve_status_id(ByVal status_name_id As map(Of String, UInt32),
                                               ByVal name As String,
                                               ByRef id As UInt32) As Boolean
        Return retrieve_map(status_name_id, name, id)
    End Function

    Private Function retrieve_action(ByVal method_type As Type,
                                     ByVal name As String,
                                     ByRef a As Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean)) As Boolean
        If String.IsNullOrEmpty(name) Then
            a = Nothing
            Return True
        Else
            Dim inv As invoker(Of Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean)) = Nothing
            inv = New invoker(Of Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean)) _
                             (method_type,
                              BindingFlags.Static Or BindingFlags.Public Or BindingFlags.NonPublic, name)
            Return inv.valid() AndAlso assert(inv.static()) AndAlso eva(a, +inv)
        End If
    End Function

    Private Function retrieve_method_type(ByRef t As Type) As Boolean
        If String.IsNullOrEmpty(i.method_type()) Then
            Return False
        Else
            Try
                t = Type.GetType(i.method_type())
                Return Not t Is Nothing
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to retrieve type ",
                            i.method_type(),
                            ", ex ",
                            ex.Message())
                Return False
            End Try
        End If
    End Function

    Private Function define_parser(ByVal type_name_id As map(Of String, Int32),
                                   ByVal status_name_id As map(Of String, UInt32),
                                   ByVal method_type As Type) As Boolean
        assert(Not type_name_id Is Nothing)
        assert(Not status_name_id Is Nothing)
        Dim it As map(Of String, map(Of String, pair(Of String, String))).iterator = Nothing
        it = i.statuses.begin()
        While it <> i.statuses.end()
            Dim f As UInt32 = 0
            If Not retrieve_status_id(status_name_id, (+it).first, f) Then
                Return False
            End If
            Dim it2 As map(Of String, pair(Of String, String)).iterator = Nothing
            it2 = (+it).second.begin()
            While it2 <> (+it).second.end()
                Dim v As Int32 = 0
                Dim t As UInt32 = 0
                Dim a As Func(Of lexer.typed_word(), UInt32, RESULT_T, Boolean) = Nothing
                If retrieve_type_id(type_name_id, (+it2).first, v) AndAlso
                   retrieve_status_id(status_name_id, (+it2).second.first, t) AndAlso
                   retrieve_action(method_type, (+it2).second.second, a) Then
                    If Not If(a Is Nothing,
                              p.define(f, v, t),
                              p.define(f, v, t, a)) Then
                        Return False
                    End If
                Else
                    Return False
                End If
                it2 += 1
            End While
            it += 1
        End While
        Return True
    End Function

    Private Function parse(ByVal inputs As vector(Of String)) As Boolean
        Dim tm As map(Of String, Int32) = Nothing
        Dim sm As map(Of String, UInt32) = Nothing
        Dim t As Type = Nothing
        Return i.parse(inputs) AndAlso
               define_lexer(tm) AndAlso
               allocate_status_name_id(sm) AndAlso
               retrieve_method_type(t) AndAlso
               define_parser(tm, sm, t)
    End Function

    Private Shared Function ctor() As Func(Of lp(Of MAX_TYPE, RESULT_T))
        Return Function() New lp(Of MAX_TYPE, RESULT_T)()
    End Function

    'protected for the inheriting classes with predefined MAX_TYPE
    Protected Shared Function ctor(Of T As lp(Of MAX_TYPE, RESULT_T)) _
                                  (ByVal c As Func(Of T),
                                   ByVal inputs As vector(Of String),
                                   ByVal ParamArray [overrides]() As String) As T
        If inputs Is Nothing OrElse
           (Not isemptyarray([overrides]) AndAlso
            Not inputs.emplace_back([overrides])) Then
            'should not happen
            Return Nothing
        Else
            assert(Not c Is Nothing)
            Dim r As T = Nothing
            r = c()
            assert(Not r Is Nothing)
            If r.parse(inputs) Then
                Return r
            Else
                Return Nothing
            End If
        End If
    End Function

    Private Shared Function read_file(ByVal file_name As String) As vector(Of String)
        Dim r As TextReader = Nothing
        Try
            r = New StreamReader(file_name)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to open file ",
                        file_name,
                        ", ex ",
                        ex.Message())
            Return Nothing
        End Try

        Dim v As vector(Of String) = Nothing
        Using r
            v = r.read_lines()
        End Using
        Return v
    End Function

    Protected Shared Function ctor(Of T As lp(Of MAX_TYPE, RESULT_T)) _
                                  (ByVal c As Func(Of T),
                                   ByVal file_name As String,
                                   ByVal ParamArray [overrides]() As String) As T
        Return ctor(c, read_file(file_name), [overrides])
    End Function

    Protected Shared Function ctor(Of T As lp(Of MAX_TYPE, RESULT_T))(ByVal c As Func(Of T),
                                                                      ByVal inputs As vector(Of String),
                                                                      ByVal mt As Type) As T
        If mt Is Nothing Then
            Return Nothing
        Else
            Return ctor(c, inputs, method_type_section_name, value_start + mt.AssemblyQualifiedName())
        End If
    End Function

    Protected Shared Function ctor(Of T As lp(Of MAX_TYPE, RESULT_T))(ByVal c As Func(Of T),
                                                                      ByVal file_name As String,
                                                                      ByVal mt As Type) As T
        Return ctor(c, read_file(file_name), mt)
    End Function

    Public Shared Function ctor(ByVal inputs As vector(Of String),
                                ByVal ParamArray [overrides]() As String) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(ctor(), inputs, [overrides])
    End Function

    Public Shared Function ctor(ByVal file_name As String,
                                ByVal ParamArray [overrides]() As String) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(read_file(file_name), [overrides])
    End Function

    Public Shared Function ctor(ByVal inputs As vector(Of String),
                                ByVal t As Type) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(ctor(), inputs, t)
    End Function

    Public Shared Function ctor(ByVal file_name As String,
                                ByVal t As Type) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(read_file(file_name), t)
    End Function

    Public Shared Function ctor(Of T)(ByVal inputs As vector(Of String)) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(ctor(), inputs, GetType(T))
    End Function

    Public Shared Function ctor(ByVal inputs As vector(Of String)) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(Of RESULT_T)(inputs)
    End Function

    Public Shared Function ctor(Of T)(ByVal file_name As String) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(Of T)(read_file(file_name))
    End Function

    Public Shared Function ctor(ByVal file_name As String) As lp(Of MAX_TYPE, RESULT_T)
        Return ctor(Of RESULT_T)(file_name)
    End Function
End Class
