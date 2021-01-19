
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public Class matching_wrapper
        Inherits matching
        Implements IComparable(Of matching_wrapper)

        Private ReadOnly m As matching

        Public Sub New(ByVal c As syntax_collection, ByVal m As matching)
            MyBase.New(c)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            Me.New(c, matching_creator.create(c, m))
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal ms() As UInt32)
            Me.New(c, matching_creator.create(c, ms))
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal m1 As UInt32,
                       ByVal m2 As UInt32,
                       ByVal ParamArray ms() As UInt32)
            Me.New(c, matching_creator.create(c, m1, m2, ms))
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of result)
            Return m.match(v, p)
        End Function

        Public Shared Operator +(ByVal this As matching_wrapper) As matching
            Return If(this Is Nothing, Nothing, this.m)
        End Operator

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of any_matching_group)().from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_wrapper) As Int32 _
                                           Implements IComparable(Of matching_wrapper).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            Return compare(Me.m, other.m)
        End Function

        Public Overrides Function ToString() As String
            Return strcat(Me.GetType().Name(), "[", m, "]")
        End Function
    End Class
End Class
