
Option Explicit On
Option Infer Off
Option Strict On

'copy from bset.vb

Public Class [set](Of T)
    Inherits bbst(Of T)
    Implements ICloneable

    Public Shared Shadows Function move(ByVal v As [set](Of T)) As [set](Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As [set](Of T) = Nothing
            r = New [set](Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As [set](Of T)
        Dim r As [set](Of T) = Nothing
        r = New [set](Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class