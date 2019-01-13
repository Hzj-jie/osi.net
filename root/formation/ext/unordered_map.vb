﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _unordered_map
    Private Function insert(Of KEY_T, VALUE_T) _
                           (ByVal this As unordered_map(Of KEY_T, VALUE_T),
                            ByVal that As unordered_map(Of KEY_T, VALUE_T),
                            ByVal f As Func(Of KEY_T,
                                               VALUE_T,
                                               pair(Of unordered_map(Of KEY_T, VALUE_T).iterator, Boolean))) As Boolean
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
End Module