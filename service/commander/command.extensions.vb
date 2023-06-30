
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _command_extensions
    <Extension()> Public Function attach(ByVal i As command, ByVal action() As Byte) As command
        If Not i Is Nothing Then
            i.set_action(action)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of T)(ByVal i As command, ByVal action As T) As command
        If Not i Is Nothing Then
            i.set_action(action)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(ByVal i As command, ByVal k() As Byte, ByVal v() As Byte) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT)(ByVal i As command, ByVal k As KT, ByVal v() As Byte) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of VT)(ByVal i As command, ByVal k() As Byte, ByVal v As VT) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT, VT)(ByVal i As command, ByVal k As KT, ByVal v As VT) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Sub renew(ByRef i As command)
        If i Is Nothing Then
            i = New command()
        Else
            i.clear()
        End If
    End Sub
End Module
