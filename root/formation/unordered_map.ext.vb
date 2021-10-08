﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public NotInheritable Class unordered_map
    Public Shared Function move(Of K, V)(ByVal i As unordered_map(Of K, V)) As unordered_map(Of K, V)
        Return unordered_map(Of K, V).move(i)
    End Function

    Public Shared Function swap(Of K, V)(ByVal i As unordered_map(Of K, V),
                                         ByVal j As unordered_map(Of K, V)) As Boolean
        Return unordered_map(Of K, V).swap(i, j)
    End Function

    Private Sub New()
    End Sub
End Class

Public Module _unordered_map
    Private Function insert(Of KEY_T, VALUE_T) _
                           (ByVal this As unordered_map(Of KEY_T, VALUE_T),
                            ByVal that As unordered_map(Of KEY_T, VALUE_T),
                            ByVal f As Func(Of KEY_T,
                                               VALUE_T,
                                               tuple(Of unordered_map(Of KEY_T, VALUE_T).iterator, Boolean))) As Boolean
        assert(Not f Is Nothing)
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
        it = that.begin()
        While it <> that.end()
            assert(f((+it).first, (+it).second).first <> this.end())
            it += 1
        End While
        Return True
    End Function

    <Extension()> Public Function insert(Of KEY_T, VALUE_T)(ByVal this As unordered_map(Of KEY_T, VALUE_T),
                                                            ByVal that As unordered_map(Of KEY_T, VALUE_T)) As Boolean
        Return insert(this, that, AddressOf this.insert)
    End Function

    <Extension()> Public Function emplace(Of KEY_T, VALUE_T)(ByVal this As unordered_map(Of KEY_T, VALUE_T),
                                                             ByVal that As unordered_map(Of KEY_T, VALUE_T)) As Boolean
        Return insert(this, that, AddressOf this.emplace)
    End Function

    <Extension()> Public Function stream(Of K, V) _
                                        (ByVal this As unordered_map(Of K, V)) As stream(Of first_const_pair(Of K, V))
        Return New stream(Of first_const_pair(Of K, V)).container(Of unordered_map(Of K, V))(this)
    End Function

    <Extension()> Public Function find(Of KT, VT) _
                                      (ByVal this As unordered_map(Of KT, VT),
                                       ByVal key As KT,
                                       ByRef o As VT) As Boolean
        If this Is Nothing Then
            Return False
        End If
        Dim it As unordered_map(Of KT, VT).iterator = this.find(key)
        If it = this.end() Then
            Return False
        End If
        o = (+it).second
        Return True
    End Function

    <Extension()> Public Function find_or_default(Of KT, VT) _
                                                 (ByVal this As unordered_map(Of KT, VT),
                                                  ByVal key As KT,
                                                  ByRef o As VT,
                                                  Optional ByVal default_value As VT = Nothing) As Boolean
        If find(this, key, o) Then
            Return True
        End If
        o = default_value
        Return False
    End Function

    <Extension()> Public Function find_or(Of KT, VT) _
                                         (ByVal this As unordered_map(Of KT, VT),
                                          ByVal key As KT,
                                          ByVal default_value As VT) As VT
        Dim o As VT = Nothing
        If find(this, key, o) Then
            Return o
        End If
        Return default_value
    End Function

    <Extension()> Public Function find_opt(Of KT, VT)(ByVal this As unordered_map(Of KT, VT),
                                                      ByVal key As KT) As [optional](Of VT)
        Dim o As VT = Nothing
        If find(this, key, o) Then
            Return [optional].of_nullable(o)
        End If
        Return [optional].empty(Of VT)()
    End Function
End Module
