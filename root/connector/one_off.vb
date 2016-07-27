
Imports System.Runtime.CompilerServices
Imports System.Threading

Public Module _one_off
    <Extension()> Public Function [get](Of T As Class)(ByVal i As one_off(Of T)) As T
        If i Is Nothing Then
            Return Nothing
        Else
            Dim x As T = Nothing
            i.get(x)
            Return x
        End If
    End Function
End Module

Public Class one_off(Of T As Class)
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
        Else
            Return Interlocked.CompareExchange(Me.i, i, Nothing) Is Nothing
        End If
    End Function

    Public Function has() As Boolean
        Return Not Me.i Is Nothing
    End Function

    Public Function [get](ByRef i As T) As Boolean
        Dim x As T = Nothing
        x = Me.i
        If x Is Nothing Then
            Return False
        Else
            If Interlocked.CompareExchange(Me.i, Nothing, x) Is x Then
                i = x
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Shared Operator +(ByVal i As one_off(Of T)) As T
        Return i.get()
    End Operator
End Class
