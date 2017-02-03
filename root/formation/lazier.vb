
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class lazier
    Public Shared Function [New](Of T)(ByVal d As Func(Of T)) As lazier(Of T)
        Return New lazier(Of T)(d)
    End Function

    Public Shared Function new_from_value(Of T)(ByVal v As T) As lazier(Of T)
        Return New lazier(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public Structure lazier(Of T)
    Private Shared ReadOnly default_value As T = Nothing
    Private d As Func(Of T)
    Private v As T

    Shared Sub New()
        default_value = Nothing
    End Sub

    Public Sub New(ByVal d As Func(Of T))
        Me.d = d
        Me.v = Nothing
    End Sub

    Public Sub New(ByVal v As T)
        Me.d = Nothing
        Me.v = v
    End Sub

    Public Overrides Function ToString() As String
        Return Convert.ToString(+Me)
    End Function

    Public Shared Widening Operator CType(ByVal this As lazier(Of T)) As T
        Return +this
    End Operator

    Public Shared Widening Operator CType(ByVal this As Func(Of T)) As lazier(Of T)
        Return New lazier(Of T)(this)
    End Operator

    Public Shared Operator +(ByVal this As lazier(Of T)) As T
        If Not this.d Is Nothing Then
            this.v = this.d()
            this.d = Nothing
        End If
        Return this.v
    End Operator
End Structure
