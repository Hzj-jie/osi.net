﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Implements ICloneable, ICloneable(Of hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER) _
                             Implements ICloneable(Of hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)).Clone
        Return clone(Of hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))()
    End Function

    Protected Function clone(Of R As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))() As R
        Return copy_constructor(Of R).invoke(v.CloneT(), s, c)
    End Function

    Protected Shared Sub move_to(ByVal f As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER),
                                 ByVal t As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))
        assert(Not f Is Nothing)
        assert(Not t Is Nothing)
        t.v = vector(Of array(Of constant(Of T))).move(f.v)
        t.s = f.s
        t.c = f.c

        f.clear()
    End Sub

    Public Shared Function move(ByVal v As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)) _
                               As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER) = Nothing
            r = _new(r)
            move_to(v, r)
            Return r
        End If
    End Function

    Protected Shared Function move(Of R As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))(ByVal v As R) As R
        If v Is Nothing Then
            Return Nothing
        Else
            Dim o As R = Nothing
            o = alloc(Of R)()
            move_to(v, o)
            Return o
        End If
    End Function

    Public Shared Function swap(ByVal this As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER),
                                ByVal that As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            assert(vector(Of array(Of constant(Of T))).swap(this.v, that.v))
            _swap.swap(this.s, that.s)
            Return True
        End If
    End Function
End Class