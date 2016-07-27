
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class syntaxer
    Public Class optional_matching_group
        Inherits matching_wrapper
        Implements IComparable(Of optional_matching_group)

        Public Sub New(ByVal m As matching)
            MyBase.New(m)
        End Sub

        Public Sub New(ByVal m As UInt32)
            MyBase.New(m)
        End Sub

        Public Sub New(ByVal ms() As UInt32)
            MyBase.New(ms)
        End Sub

        Public Sub New(ByVal m1 As UInt32, ByVal m2 As UInt32, ByVal ParamArray ms() As UInt32)
            MyBase.New(m1, m2, ms)
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            Dim op As UInt32 = 0
            op = p
            If Not MyBase.match(v, p, parent) Then
                p = op
            End If
            Return True
        End Function

        Public Overloads Function CompareTo(ByVal other As optional_matching_group) As Int32 _
                                           Implements IComparable(Of optional_matching_group).CompareTo
            Return MyBase.CompareTo(other)
        End Function
    End Class
End Class
