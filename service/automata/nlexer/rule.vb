
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class rule
        Private ReadOnly ms As vector(Of matcher)

        Public Sub New(ByVal ms As vector(Of matcher))
            assert(Not ms Is Nothing)
            assert(Not ms.empty())
            Me.ms = ms
        End Sub

        Public Sub New(ByVal ParamArray gs() As matcher)
            Me.New(vector.of(gs))
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32)
            For j As UInt32 = 0 To ms.size() - uint32_1
                Dim r As [optional](Of UInt32) = Nothing
                r = ms(j).match(i, pos)
                If Not r Then
                    Return [optional].of(Of UInt32)()
                End If
                pos = +r
            Next
            Return [optional].of(pos)
        End Function

        ' Process abc[d,e|f]+
        Public Shared Function [of](ByVal s As String) As rule

        End Function
    End Class
End Class
