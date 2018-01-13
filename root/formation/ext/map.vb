
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _map
    <Extension()> Public Function null_or_empty(Of KT, VT)(ByVal this As map(Of KT, VT)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function

    <Extension()> Public Function find(Of KT, VT) _
                                      (ByVal this As map(Of KT, VT), ByVal key As KT, ByRef o As VT) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Dim it As map(Of KT, VT).iterator = Nothing
            it = this.find(key)
            If it = this.end() Then
                Return False
            Else
                o = (+it).second
                Return True
            End If
        End If
    End Function

    <Extension()> Public Function find_or_default(Of KT, VT) _
                                                 (ByVal this As map(Of KT, VT),
                                                  ByVal key As KT,
                                                  ByRef o As VT,
                                                  Optional ByVal default_value As VT = Nothing) As Boolean
        If find(this, key, o) Then
            Return True
        Else
            o = default_value
            Return False
        End If
    End Function

    <Extension()> Public Function insert(Of KEY_T, VALUE_T)(ByVal this As map(Of KEY_T, VALUE_T),
                                                            ByVal that As map(Of KEY_T, VALUE_T)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
            it = that.begin()
            While it <> that.end()
                assert(this.insert((+it).first, (+it).second) <> this.end())
                it += 1
            End While
            Return True
        End If
    End Function

    <Extension()> Public Sub renew(Of KEY_T, VALUE_T)(ByRef this As map(Of KEY_T, VALUE_T))
        If this Is Nothing Then
            this = New map(Of KEY_T, VALUE_T)()
        Else
            this.clear()
        End If
    End Sub
End Module
