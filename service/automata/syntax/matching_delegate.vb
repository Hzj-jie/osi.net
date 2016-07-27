
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class matching_delegate
        Inherits matching
        Implements IComparable(Of matching_delegate)

        Private ReadOnly c As syntax_collection
        Private ReadOnly type As UInt32

        Public Sub New(ByVal c As syntax_collection, ByVal type As UInt32)
            assert(Not c Is Nothing)
            Me.c = c
            Me.type = type
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            Dim s As syntax = Nothing
            assert(c.get(type, s) AndAlso Not s Is Nothing)
            Return s.match(v, p, parent)
        End Function

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_delegate)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_delegate) As Int32 _
                                           Implements IComparable(Of matching_delegate).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                c = compare(Me.c, other.c)
                If c = 0 Then
                    Return compare(Me.type, other.type)
                Else
                    Return c
                End If
            Else
                Return c
            End If
        End Function
    End Class
End Class
