
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class unordered_set(Of T,
                              _HASHER As _to_uint32(Of T),
                              _COMPARER As _comparer(Of T))
    Inherits hashtable(Of T, _true, _HASHER, _COMPARER)
    Implements ICloneable, ICloneable(Of unordered_set(Of T, _HASHER, _COMPARER))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_set(Of T, _HASHER, _COMPARER))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_set(Of T, _HASHER, _COMPARER) _
            Implements ICloneable(Of unordered_set(Of T, _HASHER, _COMPARER)).Clone
        Return MyBase.clone(Of unordered_set(Of T, _HASHER, _COMPARER))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_set(Of T, _HASHER, _COMPARER)) _
                                       As unordered_set(Of T, _HASHER, _COMPARER)
        Return hashtable(Of T, _true, _HASHER, _COMPARER) _
                   .move(Of unordered_set(Of T, _HASHER, _COMPARER))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_set(Of T, _HASHER, _COMPARER),
                                        ByVal that As unordered_set(Of T, _HASHER, _COMPARER)) As Boolean
        Return hashtable(Of T, _true, _HASHER, _COMPARER).swap(this, that)
    End Function
End Class

Public Class unordered_set(Of T)
    Inherits hashtable(Of T)
    Implements ICloneable, ICloneable(Of unordered_set(Of T))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_set(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_set(Of T) _
            Implements ICloneable(Of unordered_set(Of T)).Clone
        Return MyBase.clone(Of unordered_set(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_set(Of T)) _
                                       As unordered_set(Of T)
        Return hashtable(Of T, _true, fast_to_uint32(Of T), default_comparer(Of T)) _
                  .move(Of unordered_set(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_set(Of T),
                                        ByVal that As unordered_set(Of T)) As Boolean
        Return hashtable(Of T, _true).swap(this, that)
    End Function
End Class