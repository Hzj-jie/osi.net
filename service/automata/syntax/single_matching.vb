
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class single_matching
        Inherits matching
        Implements IComparable(Of single_matching)

        <ThreadStatic> Private Shared s As [set](Of UInt32)
        Private ReadOnly m As UInt32

        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            MyBase.New(c)
            Me.m = m
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of result)
            If s Is Nothing Then
                s = New [set](Of UInt32)()
            End If
            If s.find(m) <> s.end() Then
                raise_error(error_type.user, "Cycle dependency found at ", type_name(m))
                Return [optional].empty(Of result)()
            End If
            s.emplace(m)
            Using deferring.to(Sub()
                                   assert(s.erase(m))
                               End Sub)
                If v Is Nothing OrElse v.size() <= p Then
                    Return [optional].empty(Of result)()
                End If
                assert(Not v(p) Is Nothing)
                If v(p).type <> m Then
                    Return [optional].empty(Of result)()
                End If
                Return [optional].of(New result(p + uint32_1, create_node(v, v(p).type, p, p + uint32_1)))
            End Using
        End Function

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of single_matching)().from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As single_matching) As Int32 _
                                           Implements IComparable(Of single_matching).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            Return compare(Me.m, other.m)
        End Function

        Public Overrides Function ToString() As String
            Return strcat("single_matching ", type_name(m))
        End Function
    End Class
End Class
