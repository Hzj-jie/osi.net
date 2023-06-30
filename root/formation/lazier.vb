
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class lazier
    Public Shared Function [of](Of T)(ByVal d As Func(Of T)) As lazier(Of T)
        Return New lazier(Of T)(d)
    End Function

    Public Shared Function value_of(Of T)(ByVal v As T) As lazier(Of T)
        Return New lazier(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class lazier(Of T)
    Private Shared ReadOnly default_value As T = Nothing
    Private d As Func(Of T)
    Private v As T

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

    Public Function map(Of R)(ByVal f As Func(Of T, R)) As lazier(Of R)
        assert(Not f Is Nothing)
        Return lazier.of(Function() As R
                             Return f(+Me)
                         End Function)
    End Function

    Public Shared Widening Operator CType(ByVal this As lazier(Of T)) As T
        Return +this
    End Operator

    Public Shared Widening Operator CType(ByVal this As Func(Of T)) As lazier(Of T)
        Return New lazier(Of T)(this)
    End Operator

    ' Using structure will cause this.d to be copied.
    Public Shared Operator +(ByVal this As lazier(Of T)) As T
        Dim x As Func(Of T) = Nothing
        x = this.d
        If Not x Is Nothing Then
            this.v = x()
            this.d = Nothing
        End If
        Return this.v
    End Operator
End Class
