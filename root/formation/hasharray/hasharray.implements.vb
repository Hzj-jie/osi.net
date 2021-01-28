
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Implements ICloneable, ICloneable(Of hasharray(Of T, _UNIQUE, _HASHER, _EQUALER))

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER) _
                             Implements ICloneable(Of hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)).Clone
        Return clone(Of hasharray(Of T, _UNIQUE, _HASHER, _EQUALER))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Function clone(Of R As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER))() As R
        Return copy_constructor(Of R).invoke(v.CloneT(), s, c)
    End Function

    Protected Shared Function move(Of R As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER))(ByVal v As R) As R
        If v Is Nothing Then
            Return Nothing
        End If
        Dim o As R =
            copy_constructor(Of R).invoke(array(Of vector(Of hasher_node)).move(v.v), v.s, v.c)
        v.clear()
        Return o
    End Function

    Public Shared Function move(ByVal v As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)) _
                               As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)
        Return move(Of hasharray(Of T, _UNIQUE, _HASHER, _EQUALER))(v)
    End Function

    Public Shared Function swap(ByVal this As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER),
                                ByVal that As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If

        _swap.swap(this.v, that.v)
        _swap.swap(this.s, that.s)
        _swap.swap(this.c, that.c)
        Return True
    End Function
End Class
