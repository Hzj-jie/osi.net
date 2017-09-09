
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _filtered_value
    <Extension()> Public Function values_or_null(Of T)(
                                      ByVal this As filtered_value(Of T),
                                      Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                      As vector(Of pair(Of String, T))
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.values(variants)
        End If
    End Function
End Module

Public Class filtered_value(Of T)
    Private ReadOnly m As map(Of String, vector(Of pair(Of T, filter_set)))

    Friend Sub New(ByVal fs As filter_selector, ByVal m As filtered_raw_value(Of T))
        assert(Not fs Is Nothing)
        assert(Not m Is Nothing)
        Me.m = New map(Of String, vector(Of pair(Of T, filter_set)))()
        m.foreach(Sub(ByRef key As String,
                      ByRef value As T,
                      ByRef vf As vector(Of pair(Of String, String)))
                      Me.m(key).emplace_back(emplace_make_pair(value, New filter_set(fs, vf)))
                  End Sub)
    End Sub

    Default Public ReadOnly Property v(ByVal key As String,
                                       Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                       Optional ByVal default_value As T = Nothing) As T
        Get
            Dim r As T = Nothing
            If [get](key, r, variants) Then
                Return r
            Else
                Return default_value
            End If
        End Get
    End Property

    Private Function [get](ByVal vs As vector(Of pair(Of T, filter_set)),
                           ByVal variants As vector(Of pair(Of String, String))) As vector(Of T)
        assert(Not vs Is Nothing)
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        Dim i As UInt32 = 0
        While i < vs.size()
            If vs(i).second.match(variants) Then
                r.emplace_back(vs(i).first)
            End If
            i += uint32_1
        End While
        Return r
    End Function

    Private Function [get](ByVal vs As vector(Of pair(Of T, filter_set)),
                           ByRef v As T,
                           ByVal variants As vector(Of pair(Of String, String))) As Boolean
        assert(Not vs Is Nothing)
        Dim i As UInt32 = 0
        While i < vs.size()
            If vs(i).second.match(variants) Then
                v = vs(i).first
                Return True
            End If
            i += uint32_1
        End While
        Return False
    End Function

    Public Function [get](ByVal key As String,
                          ByRef v As T,
                          Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As Boolean
        Dim it As map(Of String, vector(Of pair(Of T, filter_set))).iterator = Nothing
        it = m.find(key)
        If it = m.end() Then
            Return False
        Else
            Return [get]((+it).second, v, variants)
        End If
    End Function

    Public Function [get](ByVal key As String,
                          Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As vector(Of T)
        Dim it As map(Of String, vector(Of pair(Of T, filter_set))).iterator = Nothing
        it = m.find(key)
        If it = m.end() Then
            Return Nothing
        Else
            Return [get]((+it).second, variants)
        End If
    End Function

    Public Function exists(ByVal key As String,
                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As Boolean
        Return [get](key, Nothing, variants)
    End Function

    Public Function keys(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        r = New vector(Of String)()
        osi.root.utils.foreach(AddressOf m.foreach,
                               Sub(ByRef k As String,
                                   ByRef v As vector(Of pair(Of T, filter_set)))
                                   If [get](v, Nothing, variants) Then
                                       r.emplace_back(k)
                                   End If
                               End Sub)
        Return r
    End Function

    Public Function values(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                          As vector(Of pair(Of String, T))
        Dim r As vector(Of pair(Of String, T)) = Nothing
        r = New vector(Of pair(Of String, T))()
        osi.root.utils.foreach(AddressOf m.foreach,
                               Sub(ByRef k As String,
                                   ByRef v As vector(Of pair(Of T, filter_set)))
                                   Dim t As T = Nothing
                                   If [get](v, t, variants) Then
                                       r.emplace_back(emplace_make_pair(k, t))
                                   End If
                               End Sub)
        Return r
    End Function
End Class
