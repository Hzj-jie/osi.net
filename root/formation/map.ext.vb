
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
        End If
        Dim it As map(Of KT, VT).iterator = Nothing
        it = this.find(key)
        If it = this.end() Then
            Return False
        End If
        o = (+it).second
        Return True
    End Function

    <Extension()> Public Function find_or_default(Of KT, VT) _
                                                 (ByVal this As map(Of KT, VT),
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
                                         (ByVal this As map(Of KT, VT),
                                          ByVal key As KT,
                                          ByVal default_value As VT) As VT
        Dim o As VT = Nothing
        If find(this, key, o) Then
            Return o
        End If
        Return default_value
    End Function

    <Extension()> Public Function find_opt(Of KT, VT)(ByVal this As map(Of KT, VT),
                                                      ByVal key As KT) As [optional](Of VT)
        Dim o As VT = Nothing
        If find(this, key, o) Then
            Return [optional].of_nullable(o)
        End If
        Return [optional].empty(Of VT)()
    End Function

    Private Function insert(Of KEY_T, VALUE_T) _
                           (ByVal this As map(Of KEY_T, VALUE_T),
                            ByVal that As map(Of KEY_T, VALUE_T),
                            ByVal f As Func(Of KEY_T,
                                               VALUE_T,
                                               tuple(Of map(Of KEY_T, VALUE_T).iterator, Boolean))) As Boolean
        assert(Not f Is Nothing)
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Dim it As map(Of KEY_T, VALUE_T).iterator = that.begin()
        While it <> that.end()
            assert(f((+it).first, (+it).second).first <> this.end())
            it += 1
        End While
        Return True
    End Function

    <Extension()> Public Function insert(Of KEY_T, VALUE_T)(ByVal this As map(Of KEY_T, VALUE_T),
                                                            ByVal that As map(Of KEY_T, VALUE_T)) As Boolean
        Return insert(this, that, AddressOf this.insert)
    End Function

    <Extension()> Public Function emplace(Of KEY_T, VALUE_T)(ByVal this As map(Of KEY_T, VALUE_T),
                                                             ByVal that As map(Of KEY_T, VALUE_T)) As Boolean
        Return insert(this, that, AddressOf this.emplace)
    End Function

    <Extension()> Public Function renew(Of KEY_T, VALUE_T) _
                                       (ByRef this As map(Of KEY_T, VALUE_T)) As map(Of KEY_T, VALUE_T)
        If this Is Nothing Then
            this = New map(Of KEY_T, VALUE_T)()
        Else
            this.clear()
        End If
        Return this
    End Function

    <Extension()> Public Function stream(Of K, V)(ByVal i As map(Of K, V)) As stream(Of first_const_pair(Of K, V))
        Return New stream(Of first_const_pair(Of K, V)).container(Of map(Of K, V))(i)
    End Function
End Module

Public NotInheritable Class map
    Private Shared Function create(Of KEY_T, VALUE_T)(ByVal vs() As pair(Of KEY_T, VALUE_T),
                                                      ByVal require_copy As Boolean) As map(Of KEY_T, VALUE_T)
        Dim r As New map(Of KEY_T, VALUE_T)()
        For i As Int32 = 0 To array_size_i(vs) - 1
            If Not vs(i) Is Nothing Then
                If require_copy Then
                    r.emplace(vs(i).to_first_const_pair())
                Else
                    r.emplace(vs(i).emplace_to_first_const_pair())
                End If
            End If
        Next
        Return r
    End Function

    Public Shared Function [of](Of KEY_T, VALUE_T) _
                               (ByVal ParamArray vs() As pair(Of KEY_T, VALUE_T)) As map(Of KEY_T, VALUE_T)
        Return create(vs, True)
    End Function

    Public Shared Function emplace_of(Of KEY_T, VALUE_T) _
                                     (ByVal ParamArray vs() As pair(Of KEY_T, VALUE_T)) As map(Of KEY_T, VALUE_T)
        Return create(vs, False)
    End Function

    Public Shared Function move(Of K, V)(ByVal i As map(Of K, V)) As map(Of K, V)
        Return map(Of K, V).move(i)
    End Function

    Public Shared Function swap(Of K, V)(ByVal i As map(Of K, V), ByVal j As map(Of K, V)) As Boolean
        Return map(Of K, V).swap(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
