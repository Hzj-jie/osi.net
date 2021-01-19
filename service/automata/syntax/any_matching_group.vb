
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class any_matching_group
        Inherits matching_wrapper
        Implements IComparable(Of any_matching_group)

        Public Sub New(ByVal c As syntax_collection, ByVal m As matching)
            MyBase.New(c, m)
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            MyBase.New(c, m)
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal ms() As UInt32)
            MyBase.New(c, ms)
        End Sub

        Public Sub New(ByVal c As syntax_collection,
                       ByVal m1 As UInt32,
                       ByVal m2 As UInt32,
                       ByVal ParamArray ms() As UInt32)
            MyBase.New(c, m1, m2, ms)
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of result)
            Dim nodes As vector(Of typed_node) = Nothing
            nodes = New vector(Of typed_node)()
            Dim r As [optional](Of result) = Nothing
            r = MyBase.match(v, p)
            While r
                p = (+r).pos
                nodes.emplace_back((+r).node())
                r = MyBase.match(v, p)
            End While
            Return [optional].of(New result(p, nodes))
        End Function

        Public Overloads Function CompareTo(ByVal other As any_matching_group) As Int32 _
                                           Implements IComparable(Of any_matching_group).CompareTo
            Return MyBase.CompareTo(other)
        End Function
    End Class
End Class
