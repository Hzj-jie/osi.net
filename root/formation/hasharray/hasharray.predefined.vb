
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class hasharray(Of T, UNIQUE As _boolean)
    Inherits hasharray(Of T, UNIQUE, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of hasharray(Of T, UNIQUE))

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of constant(Of T))),
                      ByVal s As UInt32,
                      ByVal c As UInt32,
                      ByVal rc As UInt32)
        MyBase.New(v, s, c, rc)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As hasharray(Of T, UNIQUE))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hasharray(Of T, UNIQUE) Implements ICloneable(Of hasharray(Of T, UNIQUE)).Clone
        Return MyBase.clone(Of hasharray(Of T, UNIQUE))()
    End Function

    Public Shared Shadows Function move(ByVal v As hasharray(Of T, UNIQUE)) As hasharray(Of T, UNIQUE)
        Return hasharray(Of T, UNIQUE, fast_to_uint32(Of T), default_equaler(Of T)).move(Of hasharray(Of T, UNIQUE))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hasharray(Of T, UNIQUE),
                                        ByVal that As hasharray(Of T, UNIQUE)) As Boolean
        Return hasharray(Of T, UNIQUE, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class

Public Class hasharray(Of T)
    Inherits hasharray(Of T, _true)
    Implements ICloneable, ICloneable(Of hasharray(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of constant(Of T))),
                      ByVal s As UInt32,
                      ByVal c As UInt32,
                      ByVal rc As UInt32)
        MyBase.New(v, s, c, rc)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As hasharray(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hasharray(Of T) Implements ICloneable(Of hasharray(Of T)).Clone
        Return MyBase.clone(Of hasharray(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As hasharray(Of T)) As hasharray(Of T)
        Return hasharray(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of hasharray(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hasharray(Of T),
                                        ByVal that As hasharray(Of T)) As Boolean
        Return hasharray(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class

Public Class multi_hasharray(Of T)
    Inherits hasharray(Of T, _false)
    Implements ICloneable, ICloneable(Of multi_hasharray(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of constant(Of T))),
                      ByVal s As UInt32,
                      ByVal c As UInt32,
                      ByVal rc As UInt32)
        MyBase.New(v, s, c, rc)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As multi_hasharray(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As multi_hasharray(Of T) Implements ICloneable(Of multi_hasharray(Of T)).Clone
        Return MyBase.clone(Of multi_hasharray(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As multi_hasharray(Of T)) As multi_hasharray(Of T)
        Return hasharray(Of T, _false, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of multi_hasharray(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As multi_hasharray(Of T),
                                        ByVal that As multi_hasharray(Of T)) As Boolean
        Return hasharray(Of T, _false, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
