
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class syntax
        Inherits matching
        Implements IComparable(Of syntax)

        Private Const default_type As UInt32 = uint32_0
        Private Shared ReadOnly default_ignore_types As unordered_set(Of UInt32) = Nothing
        Public ReadOnly type As UInt32
        Private ReadOnly ms() As matching
        Private ReadOnly ignore_types As unordered_set(Of UInt32)

        Public Sub New(ByVal c As syntax_collection,
                       ByVal type As UInt32,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms() As matching)
            MyBase.New(c)
            Me.type = type
            Me.ignore_types = ignore_types
            assert(Not isemptyarray(ms))
            Me.ms = ms
        End Sub

        Public Shared Function create(ByVal type As UInt32,
                                      ByVal ignore_types As unordered_set(Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Dim ms As New vector(Of matching)()
            Dim m As matching = Nothing
            Dim pos As UInt32 = 0
            While pos < strlen(s)
                If Not matching_creator.create(s, collection, pos, m) Then
                    Return False
                End If
                ms.emplace_back(m)
            End While
            o = New syntax(collection, type, ignore_types, +ms)
            Return collection.set(o)
        End Function

        Public Shared Function create(ByVal type As String,
                                      ByVal ignore_types As unordered_set(Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Dim i As UInt32 = 0
            Return collection.define(type, i) AndAlso
                   create(i, ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal c As syntax_collection,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms() As matching)
            Me.New(c, default_type, ignore_types, ms)
        End Sub

        Public Shared Function create(ByVal ignore_types As unordered_set(Of UInt32),
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(default_type, ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal c As syntax_collection, ByVal type As UInt32, ByVal ParamArray ms() As matching)
            Me.New(c, type, default_ignore_types, ms)
        End Sub

        Public Shared Function create(ByVal type As UInt32,
                                      ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(type, default_ignore_types, s, collection, o)
        End Function

        Public Sub New(ByVal c As syntax_collection, ByVal ParamArray ms() As matching)
            Me.New(c, default_type, ms)
        End Sub

        Public Shared Function create(ByVal s As String,
                                      ByVal collection As syntax_collection,
                                      Optional ByRef o As syntax = Nothing) As Boolean
            Return create(default_type, s, collection, o)
        End Function

        Public Sub New(ByVal c As syntax_collection,
                       ByVal type As UInt32,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms()() As UInt32)
            Me.New(c, type, ignore_types, matching_creator.create_matchings(c, ms))
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms()() As UInt32)
            Me.New(c, default_type, ignore_types, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal type As UInt32,
                       ByVal ParamArray ms()() As UInt32)
            Me.New(c, type, default_ignore_types, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal ParamArray ms()() As UInt32)
            Me.New(c, default_type, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal type As UInt32,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms() As UInt32)
            Me.New(c, type, ignore_types, matching_creator.create_matchings(c, ms))
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal ignore_types As unordered_set(Of UInt32),
                       ByVal ParamArray ms() As UInt32)
            Me.New(c, default_type, ignore_types, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal type As UInt32,
                       ByVal ParamArray ms() As UInt32)
            Me.New(c, type, default_ignore_types, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal ParamArray ms() As UInt32)
            Me.New(c, default_type, ms)
        End Sub

        Private Function ignore_type(ByVal w As typed_word) As Boolean
            assert(Not w Is Nothing)
            Return ignore_types.find(w.type) <> ignore_types.end()
        End Function

        Private Sub jump_over_ignore_types(ByVal v As vector(Of typed_word), ByRef p As UInt32)
            If ignore_types.null_or_empty() Then
                Return
            End If
            While v.size() > p
                assert(Not v(p) Is Nothing)
                If Not ignore_type(v(p)) Then
                    Exit While
                End If
                p += uint32_1
            End While
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByVal p As UInt32) As one_of(Of result, failure)
            If v Is Nothing OrElse v.size() <= p Then
                log_end_of_tokens(v, p, Me)
                Return failure.of(p)
            End If

            Return disallow_cycle_dependency(type,
                                             p,
                                             Function() As one_of(Of result, failure)
                                                 Dim nodes As New vector(Of typed_node)()
                                                 Dim op As UInt32 = p
                                                 For i As Int32 = 0 To array_size_i(ms) - 1
                                                     jump_over_ignore_types(v, p)
                                                     Dim r As one_of(Of result, failure) = ms(i).match(v, p)
                                                     If r.is_second() Then
                                                         log_unmatched(v, p, ms(i))
                                                         Return r
                                                     End If
                                                     p = r.first().pos
                                                     nodes.emplace_back(r.first().nodes)
                                                 Next
                                                 Dim root As typed_node = create_node(v, type, op, p)
                                                 root.attach(nodes)
                                                 jump_over_ignore_types(v, p)
                                                 log_matching(v, op, p, Me)
                                                 Return result.of(p, root)
                                             End Function)
        End Function

        Public Shared Operator +(ByVal this As syntax) As matching()
            Return If(this Is Nothing, Nothing, this.ms)
        End Operator

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of syntax)().from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As syntax) As Int32 _
                                           Implements IComparable(Of syntax).CompareTo
            Dim c As Int32 = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            c = compare(Me.type, other.type)
            If c <> 0 Then
                Return c
            End If
            c = memcmp(Me.ms, other.ms)
            If c <> 0 Then
                Return c
            End If
            Return compare(Me.ignore_types, other.ignore_types)
        End Function

        Public Overrides Function ToString() As String
            Return strcat("syntax ", c.type_name(type))
        End Function
    End Class
End Class
