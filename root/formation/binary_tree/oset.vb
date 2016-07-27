
Public Class oset(Of T)
    Inherits obst(Of T)
    Implements ICloneable

    Public Shared Shadows Function move(ByVal v As oset(Of T)) As oset(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As oset(Of T) = Nothing
            r = New oset(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As oset(Of T)
        Dim r As oset(Of T) = Nothing
        r = New oset(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class
