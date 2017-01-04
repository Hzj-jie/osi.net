
Imports osi.root.connector

Public NotInheritable Class unique_ptr
    Public Shared Function [New](Of T As Class)(ByVal p As T) As unique_ptr(Of T)
        Return New unique_ptr(Of T)(p)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class unique_ptr(Of T As Class)
    Implements ICloneable, ICloneable(Of unique_ptr(Of T))

    Private p As T

    Public Sub New(ByVal p As T)
        Me.p = p
    End Sub

    Public Sub New(ByVal p As unique_ptr(Of T))
        If p Is Nothing Then
            Me.p = Nothing
        Else
            Me.p = p.p
            p.p = Nothing
        End If
    End Sub

    Public Shared Function move(ByVal this As unique_ptr(Of T)) As unique_ptr(Of T)
        Return New unique_ptr(Of T)(this)
    End Function

    Public Function empty() As Boolean
        Return p Is Nothing
    End Function

    Public Function [get]() As T
        Return p
    End Function

    Public Function release() As T
        Dim r As T = Nothing
        r = p
        p = Nothing
        Return r
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As unique_ptr(Of T) Implements ICloneable(Of unique_ptr(Of T)).Clone
        Return New unique_ptr(Of T)(Me)
    End Function

    Public Shared Widening Operator CType(ByVal this As unique_ptr(Of T)) As Boolean
        Return Not this Is Nothing AndAlso Not this.empty()
    End Operator

    Public Shared Operator +(ByVal this As unique_ptr(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.get()
        End If
    End Operator
End Class
