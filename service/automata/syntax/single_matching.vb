
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

        Private ReadOnly m As UInt32

        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            MyBase.New(c)
            Me.m = m
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByVal p As UInt32) As one_of(Of result, failure)
            If v Is Nothing OrElse v.size() <= p Then
                Return failure.of(p)
            End If
            assert(Not v(p) Is Nothing)
            If v(p).type <> m Then
                Return failure.of(p)
            End If
            Return result.of(p + uint32_1, create_node(v, v(p).type, p, p + uint32_1))
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
            Return strcat("single_matching ", c.type_name(m))
        End Function
    End Class
End Class
