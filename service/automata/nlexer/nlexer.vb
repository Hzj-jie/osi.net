
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Private ReadOnly rs As vector(Of rule)

    Public Sub New(ByVal ParamArray rs() As rule)
        Me.New(vector.of(rs))
    End Sub

    Private Sub New(ByVal rs As vector(Of rule))
        assert(Not rs Is Nothing)
        assert(Not rs.empty())
        Me.rs = rs
    End Sub

    Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of result)
        Dim j As UInt32 = 0
        While j < rs.size()
            Dim r As [optional](Of UInt32) = Nothing
            r = rs(j).match(i, pos)
            If r Then
                Return [optional].of(New result(pos, +r, j, rs(j)))
            End If
            j += uint32_1
        End While
        Return [optional].of(Of result)()
    End Function

    Public Shared Function [of](ByVal i As String, ByRef o As nlexer) As Boolean

    End Function
End Class
