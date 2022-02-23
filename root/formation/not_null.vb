
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class not_null
    Public Shared Function [New](Of T As Class)(ByVal this As T) As not_null(Of T)
        Return New not_null(Of T)(this)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class not_null(Of T As Class)
    Private ReadOnly v As T

    Public Sub New(ByVal v As T)
        assert(v IsNot Nothing)
        Me.v = v
    End Sub

    Public Function [get]() As T
        Return v
    End Function

    Public Shared Operator +(ByVal this As not_null(Of T)) As T
        assert(this IsNot Nothing)
        Return this.get()
    End Operator

    Public Shared Widening Operator CType(ByVal this As T) As not_null(Of T)
        Return not_null.[New](this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As not_null(Of T)) As T
        Return +this
    End Operator
End Class
