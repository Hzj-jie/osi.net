
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _SIZE As _int64,
                                  _HASHER As _to_uint32(Of T),
                                  _COMPARER As _comparer(Of T))
    Implements ICloneable, ICloneable(Of hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER))

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER) _
                             Implements ICloneable(Of hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER)).Clone
        Return New hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER)(v.CloneT(), s)
    End Function

    Protected Function clone(Of R As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER))() As R
        Return copy_constructor(Of R).invoke(v.CloneT(), s)
    End Function

    Public Shared Function move(ByVal v As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER)) _
                               As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER)
        If v Is Nothing Then
            Return Nothing
        Else
            Return New hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER) _
                                (vector(Of array(Of constant(Of T), _SIZE)).move(v.v), v.s)
        End If
    End Function

    Protected Shared Function move(Of R As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER))(ByVal v As R) As R
        If v Is Nothing Then
            Return Nothing
        Else
            Return copy_constructor(Of R).invoke(vector(Of array(Of constant(Of T), _SIZE)).move(v.v), v.s)
        End If
    End Function

    Public Shared Function swap(ByVal this As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER),
                                ByVal that As hashtable(Of T, _UNIQUE, _SIZE, _HASHER, _COMPARER)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            assert(vector(Of array(Of constant(Of T), _SIZE)).swap(this.v, that.v))
            _swap.swap(this.s, that.s)
            Return True
        End If
    End Function
End Class