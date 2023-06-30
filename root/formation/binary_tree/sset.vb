
Option Explicit On
Option Infer Off
Option Strict On

Public Class sset(Of T)
    Inherits sbst(Of T)
    Implements ICloneable

    Public Shared Shadows Function move(ByVal v As sset(Of T)) As sset(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As sset(Of T) = Nothing
            r = New sset(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As sset(Of T)
        Dim r As sset(Of T) = Nothing
        r = New sset(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class
