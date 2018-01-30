
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class hashset(Of T)
    Inherits hashset(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of hashset(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of [set](Of T)), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As hashset(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hashset(Of T) Implements ICloneable(Of hashset(Of T)).Clone
        Return MyBase.clone(Of hashset(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As hashset(Of T)) As hashset(Of T)
        Return hashset(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of hashset(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hashset(Of T), ByVal that As hashset(Of T)) As Boolean
        Return hashset(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
