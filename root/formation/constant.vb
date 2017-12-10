
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _constant
    <Extension()> Public Function get_or_null(Of T)(ByVal this As constant(Of T)) As T
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.get()
        End If
    End Function
End Module

Public NotInheritable Class constant
    Public Shared Function [New](Of T)(ByVal v As T) As constant(Of T)
        Return New constant(Of T)(v)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class constant(Of T)
    Private ReadOnly v As T

    Public Sub New(ByVal v As T)
        Me.v = v
    End Sub

    Public Function [get]() As T
        Return v
    End Function

    Public Function empty() As Boolean
        Return v Is Nothing
    End Function

    Public Shared Operator +(ByVal this As constant(Of T)) As T
        Return this.get_or_null()
    End Operator

    Public Shared Widening Operator CType(ByVal this As constant(Of T)) As T
        Return +this
    End Operator

    Public Shared Widening Operator CType(ByVal this As T) As constant(Of T)
        Return constant.[New](this)
    End Operator

    Public Overrides Function ToString() As String
        Return strcat("constant(", Convert.ToString(v), ")")
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return If(v Is Nothing, 0, v.GetHashCode())
    End Function
End Class
