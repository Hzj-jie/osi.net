
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

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

    Private Function insert(Of KEY_T, VALUE_T) _
                           (ByVal this As map(Of KEY_T, VALUE_T),
                            ByVal that As map(Of KEY_T, VALUE_T),
                            ByVal f As Func(Of KEY_T,
                                               VALUE_T,
                                               pair(Of map(Of KEY_T, VALUE_T).iterator, Boolean))) As Boolean
        assert(Not f Is Nothing)
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
        it = that.begin()
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

    <Extension()> Public Sub renew(Of KEY_T, VALUE_T)(ByRef this As map(Of KEY_T, VALUE_T))
        If this Is Nothing Then
            this = New map(Of KEY_T, VALUE_T)()
        Else
            this.clear()
        End If
    End Sub

    Private Function reverse(Of KEY_T, VALUE_T)(ByVal i As map(Of KEY_T, VALUE_T),
                                                ByRef o As map(Of VALUE_T, KEY_T),
                                                ByVal copy_required As Boolean) As Boolean
        If i Is Nothing Then
            o = Nothing
            Return True
        End If

        o.renew()
        Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
        it = i.begin()
        While it <> i.end()
            Dim k As KEY_T = Nothing
            Dim v As VALUE_T = Nothing
            k = (+it).first
            v = (+it).second
            If copy_required Then
                k = copy(k)
                v = copy(v)
            End If
            If o.find(v) <> o.end() Then
                Return False
            End If
            o.emplace(v, k)
            it += 1
        End While
        Return True
    End Function

    <Extension()> Public Function emplace_reverse(Of KEY_T, VALUE_T)(ByVal i As map(Of KEY_T, VALUE_T),
                                                                     ByRef o As map(Of VALUE_T, KEY_T)) As Boolean
        Return reverse(i, o, False)
    End Function

    <Extension()> Public Function emplace_reverse(Of KEY_T, VALUE_T) _
                                                 (ByVal i As map(Of KEY_T, VALUE_T)) As map(Of VALUE_T, KEY_T)
        Dim o As map(Of VALUE_T, KEY_T) = Nothing
        assert(emplace_reverse(i, o))
        Return o
    End Function

    <Extension()> Public Function reverse(Of KEY_T, VALUE_T)(ByVal i As map(Of KEY_T, VALUE_T),
                                                             ByRef o As map(Of VALUE_T, KEY_T)) As Boolean
        Return reverse(i, o, True)
    End Function

    <Extension()> Public Function reverse(Of KEY_T, VALUE_T) _
                                         (ByVal i As map(Of KEY_T, VALUE_T)) As map(Of VALUE_T, KEY_T)
        Dim o As map(Of VALUE_T, KEY_T) = Nothing
        assert(reverse(i, o))
        Return o
    End Function
End Module

Public NotInheritable Class map
    Private Shared Function create(Of KEY_T, VALUE_T)(ByVal vs() As pair(Of KEY_T, VALUE_T),
                                                      ByVal require_copy As Boolean) As map(Of KEY_T, VALUE_T)
        Dim r As map(Of KEY_T, VALUE_T) = Nothing
        r = New map(Of KEY_T, VALUE_T)()
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

    Private Shared Function create_index(Of KEY_T)(ByVal vs() As KEY_T,
                                                   ByVal require_copy As Boolean) As map(Of KEY_T, UInt32)
        Dim r As map(Of KEY_T, UInt32) = Nothing
        r = New map(Of KEY_T, UInt32)()
        Dim i As UInt32 = 0
        While i < array_size(vs)
            Dim v As KEY_T = Nothing
            v = vs(CInt(i))
            If require_copy Then
                v = copy(v)
            End If
            r.emplace(v, i)
            i += uint32_1
        End While
        Return r
    End Function

    Public Shared Function index(Of KEY_T)(ByVal ParamArray vs() As KEY_T) As map(Of KEY_T, UInt32)
        Return create_index(vs, True)
    End Function

    Public Shared Function emplace_index(Of KEY_T)(ByVal ParamArray vs() As KEY_T) As map(Of KEY_T, UInt32)
        Return create_index(vs, False)
    End Function

    Private Sub New()
    End Sub
End Class
