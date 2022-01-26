
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class multi_matching_group
        Inherits matching_wrapper
        Implements IComparable(Of multi_matching_group)

        Public Sub New(ByVal c As syntax_collection, ByVal m As matching)
            MyBase.New(c, m)
        End Sub

        '@VisibleForTesting
        Public Sub New(ByVal c As syntax_collection, ByVal m As UInt32)
            MyBase.New(c, m)
        End Sub

        '@VisibleForTesting
        Public Sub New(ByVal c As syntax_collection, ByVal ms() As UInt32)
            MyBase.New(c, ms)
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As result
            Dim nodes As New vector(Of typed_node)()
            Dim r As result = result.failure(p)
            While True
                Dim c As result = MyBase.match(v, p)
                r = r Or c
                If c.failed() Then
                    Exit While
                End If
                p = c.suc.pos
                nodes.emplace_back(c.suc.nodes)
            End While
            If nodes.empty() Then
                assert(r.failed())
                Return r
            End If
            Return result.success(p, nodes) Or r
        End Function

        Public Overloads Function CompareTo(ByVal other As multi_matching_group) As Int32 _
                                           Implements IComparable(Of multi_matching_group).CompareTo
            Return MyBase.CompareTo(other)
        End Function
    End Class
End Class
