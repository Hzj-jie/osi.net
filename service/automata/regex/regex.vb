
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class rlexer
    Partial Public Class regex
        Implements matching_group

        Private ReadOnly gs As vector(Of matching_group)

        Public Sub New(ByVal ParamArray gs() As matching_group)
            Me.gs = New vector(Of matching_group)()
            Me.gs.emplace_back(gs)
        End Sub

        Private Sub New(ByVal gs As vector(Of matching_group))
            assert(Not gs Is Nothing)
            Me.gs = gs
        End Sub

        Private Function match(ByVal i As String,
                               ByVal poses As vector(Of UInt32),
                               ByVal index As UInt32) As vector(Of UInt32)
            assert(Not gs.empty())
            assert(index >= 0 AndAlso index < gs.size())
            assert(Not poses.null_or_empty())
            assert(Not gs(index) Is Nothing)
            Dim o As [set](Of UInt32) = Nothing
            o = New [set](Of UInt32)()
            For j As UInt32 = 0 To poses.size() - uint32_1
                Dim c As vector(Of UInt32) = Nothing
                c = gs(index).match(i, poses(j))
                assert(c Is Nothing OrElse o.emplace(c))
            Next
            Return o.stream().collect_to(Of vector(Of UInt32))()
        End Function

        Public Function match(ByVal i As String,
                              ByVal pos As UInt32) As vector(Of UInt32) Implements matching_group.match
            If gs.empty() Then
                Return Nothing
            Else
                Dim v As vector(Of UInt32) = Nothing
                v = New vector(Of UInt32)()
                v.emplace_back(pos)
                For j As UInt32 = 0 To gs.size() - uint32_1
                    v = match(i, v, j)
                    If v.null_or_empty() Then
                        Return Nothing
                    End If
                Next
                Return v
            End If
        End Function

        Public Shared Function create(ByVal i As String, ByRef o As regex) As Boolean
            If i.null_or_empty() Then
                Return False
            Else
                Dim gs As vector(Of matching_group) = Nothing
                gs = New vector(Of matching_group)()
                Dim p As UInt32 = 0
                While p < strlen(i)
                    Dim g As matching_group = Nothing
                    If matching_group_creator.create(i, p, g) Then
                        gs.emplace_back(g)
                    Else
                        Return False
                    End If
                End While
                If p = strlen(i) Then
                    o = New regex(gs)
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Shared Operator +(ByVal this As regex) As vector(Of matching_group)
            Return If(this Is Nothing, Nothing, this.gs)
        End Operator
    End Class
End Class
