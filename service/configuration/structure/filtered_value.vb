
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
        m.foreach(Sub(ByVal key As String,
                      ByVal value As T,
                      ByVal vf As vector(Of pair(Of String, String)))
                      Me.m(key).emplace_back(pair.emplace_of(value, New filter_set(fs, vf)))
                  End Sub)
    End Sub

    Default Public ReadOnly Property v(ByVal key As String,
                                       Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                       Optional ByVal default_value As T = Nothing) As T
        Get
            Dim r As T = Nothing
            If [get](key, r, variants) Then
                Return r
            End If
            Return default_value
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
        End If
        Return [get]((+it).second, v, variants)
    End Function

    Public Function [get](ByVal key As String,
                          Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As vector(Of T)
        Dim it As map(Of String, vector(Of pair(Of T, filter_set))).iterator = Nothing
        it = m.find(key)
        If it = m.end() Then
            Return Nothing
        End If
        Return [get]((+it).second, variants)
    End Function

    Public Function exists(ByVal key As String,
                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As Boolean
        Return [get](key, Nothing, variants)
    End Function

    Public Function keys(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        r = New vector(Of String)()
        m.stream().foreach(m.on_pair(Sub(ByVal k As String, ByVal v As vector(Of pair(Of T, filter_set)))
                                         If [get](v, Nothing, variants) Then
                                             r.emplace_back(k)
                                         End If
                                     End Sub))
        Return r
    End Function

    Public Function values(Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                          As vector(Of pair(Of String, T))
        Dim r As vector(Of pair(Of String, T)) = Nothing
        r = New vector(Of pair(Of String, T))()
        m.stream().foreach(m.on_pair(Sub(ByVal k As String, ByVal v As vector(Of pair(Of T, filter_set)))
                                         Dim t As T = Nothing
                                         If [get](v, t, variants) Then
                                             r.emplace_back(pair.emplace_of(k, t))
                                         End If
                                     End Sub))
        Return r
    End Function
End Class
