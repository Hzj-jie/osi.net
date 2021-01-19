
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class rlexer
    Public Class any_matching_group
        Inherits matching_group_wrapper

        Public Sub New(ByVal g As matching_group)
            MyBase.New(g)
        End Sub

        Public Overrides Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
            Dim r As [set](Of UInt32) = Nothing
            r = New [set](Of UInt32)()
            Dim q As queue(Of UInt32) = Nothing
            q = New queue(Of UInt32)()
            q.push(pos)
            While Not q.empty()
                Dim np As UInt32 = 0
                np = q.front()
                assert(q.pop())
                Dim t As vector(Of UInt32) = Nothing
                t = MyBase.match(i, np)
                If Not t.null_or_empty() Then
                    For j As UInt32 = 0 To t.size() - uint32_1
                        If r.insert(t(j)).second Then
                            assert(q.push(t(j)))
                        End If
                    Next
                End If
            End While
            r.insert(pos)
            Return r.stream().collect(Of vector(Of UInt32))()
        End Function
    End Class
End Class
