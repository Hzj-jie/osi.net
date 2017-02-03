
Option Explicit On
Option Infer Off
Option Strict On

Public Class bset(Of T)
    Inherits bbst(Of T)
    Implements ICloneable

    Public Shared Shadows Function move(ByVal v As bset(Of T)) As bset(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As bset(Of T) = Nothing
            r = New bset(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As bset(Of T)
        Dim r As bset(Of T) = Nothing
        r = New bset(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class
