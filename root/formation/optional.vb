
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class [optional]
    Public Shared Function [New](Of T)(ByVal v As T) As [optional](Of T)
        Return New [optional](Of T)(v)
    End Function

    Public Shared Function [New](Of T)() As [optional](Of T)
        Return empty(Of T)()
    End Function

    Public Shared Function [of](Of T)(ByVal v As T) As [optional](Of T)
        Return [New](v)
    End Function

    Public Shared Function [of](Of T)() As [optional](Of T)
        Return empty(Of T)()
    End Function

    Public Shared Function [empty](Of T)() As [optional](Of T)
        Return empty_cache(Of T).v
    End Function

    Private Sub New()
    End Sub

    Private NotInheritable Class empty_cache(Of T)
        Public Shared ReadOnly v As [optional](Of T)

        Shared Sub New()
            v = New [optional](Of T)()
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class

Public NotInheritable Class [optional](Of T)
    Private ReadOnly b As Boolean
    Private ReadOnly v As T

    Private Sub New(ByVal b As Boolean, ByVal v As T)
        Me.b = b
        Me.v = v
    End Sub

    Public Sub New()
        Me.New(False, Nothing)
    End Sub

    Public Sub New(ByVal v As T)
        Me.New(True, v)
    End Sub

    Public Shared Widening Operator CType(ByVal this As [optional](Of T)) As Boolean
        Return Not this Is Nothing AndAlso this.b
    End Operator

    Public Shared Operator Not(ByVal this As [optional](Of T)) As Boolean
        Return this Is Nothing OrElse Not this.b
    End Operator

    Public Shared Operator +(ByVal this As [optional](Of T)) As T
        assert(this)
        Return this.v
    End Operator

    Public Shared Operator -(ByVal this As [optional](Of T)) As T
        If this Then
            Return this.v
        Else
            Return Nothing
        End If
    End Operator
End Class
