
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class hasharray(Of T)
    Inherits hasharray(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of hasharray(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of constant(Of T)), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hasharray(Of T) Implements ICloneable(Of hasharray(Of T)).Clone
        Return MyBase.clone(Of hasharray(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As hasharray(Of T)) As hasharray(Of T)
        Return hasharray(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)).move(Of hasharray(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hasharray(Of T),
                                        ByVal that As hasharray(Of T)) As Boolean
        Return hasharray(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
