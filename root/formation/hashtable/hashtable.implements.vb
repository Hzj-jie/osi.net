
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

    Public Shared Function move(ByVal v As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)) _
                               As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)
        Return move(Of hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))(v)
    End Function

    Protected Shared Function move(Of R As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER))(ByVal v As R) As R
        If v Is Nothing Then
            Return Nothing
        End If

        Dim o As R = Nothing
        o = copy_constructor(Of R).invoke(vector(Of array(Of constant(Of T))).move(v.v), v.s, v.c)
        v.clear()
        Return o
    End Function

    Public Shared Function swap(ByVal this As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER),
                                ByVal that As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If

        _swap.swap(this.v, that.v)
        _swap.swap(this.s, that.s)
        _swap.swap(this.c, that.c)
        Return True
    End Function
End Class