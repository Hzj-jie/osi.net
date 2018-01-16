
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _command_extensions
    <Extension()> Public Function attach(ByVal i As command, ByVal action() As Byte) As command
        If Not i Is Nothing Then
            i.set_action(action)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of T)(ByVal i As command,
                                               ByVal action As T,
                                               Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) As command
        If Not i Is Nothing Then
            i.set_action(action, T_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(ByVal i As command,
                                         ByVal k() As Byte,
                                         ByVal v() As Byte) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT)(ByVal i As command,
                                                ByVal k As KT,
                                                ByVal v() As Byte,
                                                Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, KT_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of VT)(ByVal i As command,
                                                ByVal k() As Byte,
                                                ByVal v As VT,
                                                Optional ByVal VT_bytes As bytes_serializer(Of VT) = Nothing) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, VT_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT, VT) _
                                        (ByVal i As command,
                                         ByVal k As KT,
                                         ByVal v As VT,
                                         Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing,
                                         Optional ByVal VT_bytes As bytes_serializer(Of VT) = Nothing) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, KT_bytes, VT_bytes)
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
