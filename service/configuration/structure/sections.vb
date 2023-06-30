
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation

Public Module _sections
    <Extension()> Public Function values_or_null(
                                      ByVal this As sections,
                                      Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                      As vector(Of pair(Of String, String))
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.values(variants)
        End If
    End Function
End Module

' A thin wrapper of vector(Of section)
Public Class sections
    Private ReadOnly v As vector(Of section)

    Friend Sub New(ByVal v As vector(Of section))
        Me.v = v
    End Sub

    Default Public ReadOnly Property value(ByVal key As String,
                                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                           Optional ByVal default_value As String = Nothing) As String
        Get
            Dim r As String = Nothing
            If [get](key, r, variants) Then
                Return r
            Else
                Return default_value
            End If
        End Get
    End Property

    Public Function [get](ByVal key As String,
                          ByRef r As String,
                          Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As Boolean
        If v.null_or_empty() Then
            Return False
        Else
            Dim i As UInt32 = 0
            While i < v.size()
                If v(i).get(key, r, variants) Then
                    Return True
                End If
                i += uint32_1
            End While
            Return False
        End If
    End Function

    Public Function exists(ByVal key As String,
                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As Boolean
        Return [get](key, Nothing, variants)
    End Function

    Public Function keys(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As vector(Of String)
        If v.null_or_empty() Then
            Return Nothing
        Else
            Dim r As vector(Of String) = Nothing
            r = New vector(Of String)()
            Dim i As UInt32 = 0
            While i < v.size()
                r.emplace_back(v(i).keys(variants))
                i += uint32_1
            End While
            Return r
        End If
    End Function

    Public Function values(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                          As vector(Of pair(Of String, String))
        If v.null_or_empty() Then
            Return Nothing
        Else
            Dim r As vector(Of pair(Of String, String)) = Nothing
            r = New vector(Of pair(Of String, String))()
            Dim i As UInt32 = 0
            While i < v.size()
                r.emplace_back(v(i).values(variants))
                i += uint32_1
            End While
            Return r
        End If
    End Function
End Class
