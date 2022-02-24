
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports System.Threading

Public NotInheritable Class one_off
    Public Shared Function [New](Of T As Class)(ByVal i As T) As one_off(Of T)
        Return New one_off(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class one_off(Of T As Class)
    Private i As T

    Public Sub New(ByVal i As T)
        Me.i = i
    End Sub

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Function [set](ByVal i As T) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Return Interlocked.CompareExchange(Me.i, i, Nothing) Is Nothing
    End Function

    Public Shared Widening Operator CType(ByVal this As one_off(Of T)) As Boolean
        Return Not this Is Nothing AndAlso this.has()
    End Operator

    Public Function has() As Boolean
        Return Not i Is Nothing
    End Function

    Public Function [get](ByRef i As T) As Boolean
        Dim x As T = Me.i
        If x Is Nothing Then
            Return False
        End If
        If Interlocked.CompareExchange(Me.i, Nothing, x) Is x Then
            i = x
            Return True
        End If
        Return False
    End Function

    Public Function [get]() As T
        Dim x As T = Nothing
        assert([get](x))
        Return x
    End Function

    Public Shared Operator +(ByVal i As one_off(Of T)) As T
        If i Is Nothing Then
            Return Nothing
        End If
        Dim x As T = Nothing
        i.get(x)
        Return x
    End Operator

    Public Overrides Function ToString() As String
        Dim x As T = Nothing
        If [get](x) Then
            Return x.ToString()
        End If
        Return Convert.ToString(x)
    End Function
End Class
