
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Public NotInheritable Class coupling_ratio
    Public Enum mean_type
        quadratic
        arithmetic
        geometric
        harmonic
    End Enum

    Private Shared percentile_ratio As argument(Of Double)
    Private Shared mean As argument(Of mean_type)

    Private Shared Function perc_ratio() As Double
        Dim v As Double = (percentile_ratio Or 0.5)
        assert(v <= 1 AndAlso v >= 0)
        Return v
    End Function

    Public Shared Function execute(Of T)(ByVal i As onebound(Of T).model) As onebound(Of T).model
        assert(Not i Is Nothing)

    End Function

    Private Sub New()
    End Sub
End Class
