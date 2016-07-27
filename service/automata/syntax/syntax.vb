
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class syntax
        Inherits matching
        Implements IComparable(Of syntax)

        Private Const default_type As UInt32 = uint32_0
        Private Shared ReadOnly default_ignore_types As [set](Of UInt32)
        Public ReadOnly type As UInt32
        Private ReadOnly ms() As matching
        Private ReadOnly ignore_types As [set](Of UInt32)

        Shared Sub New()
            default_ignore_types = Nothing
        End Sub

        Public Sub New(ByVal type As UInt32, ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms() As matching)
            Me.type = type
            Me.ignore_types = ignore_types
            assert(Not isemptyarray(ms))
            Me.ms = ms
        End Sub

        Public Shared Function create(ByVal type As UInt32,
                                      ByVal ignore_types As [set](Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Dim ms As vector(Of matching) = Nothing
            ms = New vector(Of matching)()
            Dim m As matching = Nothing
            Dim pos As UInt32 = 0
            While pos < strlen(s)
                If matching_creator.create(s, collection, pos, m) Then
                    ms.emplace_back(m)
                Else
                    Return False
                End If
            End While
            o = New syntax(type, ignore_types, +ms)
            Return collection.set(o)
        End Function

        Public Shared Function create(ByVal type As String,
                                      ByVal ignore_types As [set](Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Dim i As UInt32 = 0
            Return collection.define(type, i) AndAlso
                   create(i, ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms() As matching)
            Me.New(default_type, ignore_types, ms)
        End Sub

        Public Shared Function create(ByVal ignore_types As [set](Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(default_type, ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal type As UInt32, ByVal ParamArray ms() As matching)
            Me.New(type, default_ignore_types, ms)
        End Sub

        Public Shared Function create(ByVal type As UInt32,
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(type, default_ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal ParamArray ms() As matching)
            Me.New(default_type, ms)
        End Sub

        Public Shared Function create(ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(default_type, s, collection, o)
        End Function

        Public Sub New(ByVal type As UInt32, ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms()() As UInt32)
            Me.New(type, ignore_types, matching_creator.create_matchings(ms))
        End Sub

        Public Sub New(ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms()() As UInt32)
            Me.New(default_type, ignore_types, ms)
        End Sub

        Public Sub New(ByVal type As UInt32, ByVal ParamArray ms()() As UInt32)
            Me.New(type, default_ignore_types, ms)
        End Sub

        Public Sub New(ByVal ParamArray ms()() As UInt32)
            Me.New(default_type, ms)
        End Sub

        Public Sub New(ByVal type As UInt32, ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms() As UInt32)
            Me.New(type, ignore_types, matching_creator.create_matchings(ms))
        End Sub

        Public Sub New(ByVal ignore_types As [set](Of UInt32), ByVal ParamArray ms() As UInt32)
            Me.New(default_type, ignore_types, ms)
        End Sub

        Public Sub New(ByVal type As UInt32, ByVal ParamArray ms() As UInt32)
            Me.New(type, default_ignore_types, ms)
        End Sub

        Public Sub New(ByVal ParamArray ms() As UInt32)
            Me.New(default_type, ms)
        End Sub

        Private Sub jump_over_ignore_types(ByVal v As vector(Of typed_word), ByRef p As UInt32)
            If Not ignore_types.null_or_empty() Then
                While v.size() > p
                    assert(Not v(p) Is Nothing)
                    If ignore_types.find(v(p).type) <> ignore_types.end() Then
                        p += 1
                    Else
                        Exit While
                    End If
                End While
            End If
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            If v Is Nothing OrElse v.size() <= p Then
                Return False
            Else
                Dim op As UInt32 = 0
                op = p
                For round As UInt32 = 0 To If(parent Is Nothing, 0, 1)
                    If round = 1 Then
                        assert(Not parent Is Nothing)
                        parent = add_subnode(v, parent, type, op, p)
                    End If
                    p = op
                    For i As UInt32 = 0 To array_size(ms) - uint32_1
                        jump_over_ignore_types(v, p)
                        If round = 0 Then
                            If Not ms(i).match(v, p) Then
                                Return False
                            End If
                        Else
                            assert(ms(i).match(v, p, parent))
                        End If
                    Next
                Next
                jump_over_ignore_types(v, p)
                Return True
            End If
        End Function

        Public Shared Operator +(ByVal this As syntax) As matching()
            Return If(this Is Nothing, Nothing, this.ms)
        End Operator

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of syntax)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As syntax) As Int32 _
                                           Implements IComparable(Of syntax).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                c = compare(Me.type, other.type)
                If c = 0 Then
                    c = memcmp(Me.ms, other.ms)
                    If c = 0 Then
                        Return compare(Me.ignore_types, other.ignore_types)
                    Else
                        Return c
                    End If
                Else
                    Return c
                End If
            Else
                Return c
            End If
        End Function
    End Class
End Class
