
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class rule
        Private ReadOnly gs As vector(Of group)

        Public Sub New(ByVal gs As vector(Of group))
            assert(Not gs Is Nothing)
            assert(Not gs.empty())
            Me.gs = gs
        End Sub

        Public Sub New(ByVal ParamArray gs() As group)
            Me.New(vector.of(gs))
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32)
            For j As UInt32 = 0 To gs.size() - uint32_1
                Dim r As [optional](Of UInt32) = Nothing
                r = gs(j).match(i, pos)
                If Not r Then
                    Return [optional].of(Of UInt32)()
                End If
                pos = +r
            Next
            Return [optional].of(pos)
        End Function
    End Class
End Class
