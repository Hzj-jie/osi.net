
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class matching_delegate
        Inherits matching
        Implements IComparable(Of matching_delegate)

        Private ReadOnly type As UInt32

        Public Sub New(ByVal c As syntax_collection, ByVal type As UInt32)
            MyBase.New(c)
            Me.type = type
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As result
            Return syntax().match(v, p)
        End Function

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_delegate)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_delegate) As Int32 _
                                           Implements IComparable(Of matching_delegate).CompareTo
            Dim c As Int32 = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            c = compare(Me.c, other.c)
            If c <> 0 Then
                Return c
            End If
            Return compare(Me.type, other.type)
        End Function

        Private Function syntax() As syntax
            Dim s As syntax = Nothing
            assert(c.get(type, s) AndAlso Not s Is Nothing)
            Return s
        End Function

        Public Overrides Function ToString() As String
            Return syntax().ToString()
        End Function
    End Class
End Class
