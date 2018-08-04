
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class hashtable(Of T, _UNIQUE As _boolean)
    Inherits hashtable(Of T, _UNIQUE, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of hashtable(Of T, _UNIQUE))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As hashtable(Of T, _UNIQUE))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hashtable(Of T, _UNIQUE) _
                                     Implements ICloneable(Of hashtable(Of T, _UNIQUE)).Clone
        Return MyBase.clone(Of hashtable(Of T, _UNIQUE))()
    End Function

    Public Shared Shadows Function move(ByVal v As hashtable(Of T, _UNIQUE)) _
                                       As hashtable(Of T, _UNIQUE)
        Return hashtable(Of T, _UNIQUE, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of hashtable(Of T, _UNIQUE))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hashtable(Of T, _UNIQUE),
                                        ByVal that As hashtable(Of T, _UNIQUE)) As Boolean
        Return hashtable(Of T, _UNIQUE, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class

Public Class hashtable(Of T)
    Inherits hashtable(Of T, _true)
    Implements ICloneable, ICloneable(Of hashtable(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As hashtable(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As hashtable(Of T) Implements ICloneable(Of hashtable(Of T)).Clone
        Return MyBase.clone(Of hashtable(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As hashtable(Of T)) As hashtable(Of T)
        Return hashtable(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of hashtable(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As hashtable(Of T),
                                        ByVal that As hashtable(Of T)) As Boolean
        Return hashtable(Of T, _true, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class

Public Class multi_hashtable(Of T)
    Inherits hashtable(Of T, _false)
    Implements ICloneable, ICloneable(Of multi_hashtable(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As multi_hashtable(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As multi_hashtable(Of T) Implements ICloneable(Of multi_hashtable(Of T)).Clone
        Return MyBase.clone(Of multi_hashtable(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As multi_hashtable(Of T)) As multi_hashtable(Of T)
        Return hashtable(Of T, _false, fast_to_uint32(Of T), default_equaler(Of T)) _
                   .move(Of multi_hashtable(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As multi_hashtable(Of T),
                                        ByVal that As multi_hashtable(Of T)) As Boolean
        Return hashtable(Of T, _false, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
