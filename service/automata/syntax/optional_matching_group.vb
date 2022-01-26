
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class optional_matching_group
        Inherits matching_wrapper
        Implements IComparable(Of optional_matching_group)

        Public Sub New(ByVal c As syntax_collection, ByVal m As matching)
            MyBase.New(c, m)
        End Sub

        ' @VisibleForTesting
        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            MyBase.New(c, m)
        End Sub

        '@VisibleForTesting
        Public Sub New(ByVal c As syntax_collection, ByVal ms() As UInt32)
            MyBase.New(c, ms)
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByVal p As UInt32) As one_of(Of result, failure)
            Dim r As one_of(Of result, failure) = MyBase.match(v, p)
            If r.is_first() Then
                Return r
            End If
            Return result.of(p)
        End Function

        Public Overloads Function CompareTo(ByVal other As optional_matching_group) As Int32 _
                                           Implements IComparable(Of optional_matching_group).CompareTo
            Return MyBase.CompareTo(other)
        End Function
    End Class
End Class
