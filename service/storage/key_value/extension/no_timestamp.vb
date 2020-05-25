
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.procedure

Public Module _no_timestamp
    Private ReadOnly read_ts As pointer(Of Int64)

    Sub New()
        read_ts = New pointer(Of Int64)()
    End Sub

    <Extension()> Public Function append(ByVal this As istrkeyvt,
                                         ByVal key As String,
                                         ByVal value() As Byte,
                                         ByVal result As pointer(Of Boolean)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.append(key, value, now_as_timestamp(), result)
        End If
    End Function

    <Extension()> Public Function modify(ByVal this As istrkeyvt,
                                         ByVal key As String,
                                         ByVal value() As Byte,
                                         ByVal result As pointer(Of Boolean)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.modify(key, value, now_as_timestamp(), result)
        End If
    End Function

    <Extension()> Public Function unique_write(ByVal this As istrkeyvt,
                                               ByVal key As String,
                                               ByVal value() As Byte,
                                               ByVal result As pointer(Of Boolean)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.unique_write(key, value, now_as_timestamp(), result)
        End If
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As pointer(Of Byte())) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.read(key, result, read_ts)
        End If
    End Function
End Module
