
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Partial Public Class unordered_map(Of KEY_T,
                                      VALUE_T,
                                      _HASHER As _to_uint32(Of KEY_T),
                                      _EQUALER As _equaler(Of KEY_T))
    Inherits hashtable(Of first_const_pair(Of KEY_T, VALUE_T), 
                          _true, 
                          first_const_pair_hasher, 
                          first_const_pair_equaler)
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER) _
            Implements ICloneable(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)).Clone
        Return clone(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) _
                                       As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T), 
                            _true, 
                            first_const_pair_hasher, 
                            first_const_pair_equaler) _
                   .move(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER),
                                        ByVal that As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) As Boolean
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T), 
                            _true, 
                            first_const_pair_hasher, 
                            first_const_pair_equaler) _
                   .swap(this, that)
    End Function

    Public Overloads Function emplace(ByVal key As KEY_T, ByVal value As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.emplace(emplace_make_first_const_pair(key, value))
    End Function

    Public Overloads Function insert(ByVal key As KEY_T, ByVal value As VALUE_T) As pair(Of iterator, Boolean)
        Return emplace(copy_no_error(key), copy_no_error(value))
    End Function

    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        Return MyBase.erase(it)
    End Function

    Public Shadows Function [erase](ByVal key As KEY_T) As Boolean
        Return MyBase.erase(find(key))
    End Function

    Public Shadows Function find(ByVal key As KEY_T) As iterator
        Return MyBase.find(emplace_make_first_const_pair(Of KEY_T, VALUE_T)(key))
    End Function

    Default Public Property at(ByVal key As KEY_T) As VALUE_T
        Get
            Dim r As iterator = Nothing
            r = find(key)
            If r = [end]() Then
                r = emplace(copy_no_error(key), alloc(Of VALUE_T)()).first
            End If
            Return (+r).second
        End Get
        Set(ByVal value As VALUE_T)
            Dim r As pair(Of iterator, Boolean) = Nothing
            r = insert(key, value)
            If Not r.second Then
                copy(r.first.value().second, value)
            End If
        End Set
    End Property
End Class

Public Class unordered_map(Of KEY_T, VALUE_T)
    Inherits unordered_map(Of KEY_T, VALUE_T, fast_to_uint32(Of KEY_T), default_equaler(Of KEY_T))
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T))

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_map(Of KEY_T, VALUE_T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_map(Of KEY_T, VALUE_T) _
            Implements ICloneable(Of unordered_map(Of KEY_T, VALUE_T)).Clone
        Return MyBase.clone(Of unordered_map(Of KEY_T, VALUE_T))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map(Of KEY_T, VALUE_T)) _
                                       As unordered_map(Of KEY_T, VALUE_T)
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T), _true, first_const_pair_hasher, first_const_pair_equaler) _
                   .move(Of unordered_map(Of KEY_T, VALUE_T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map(Of KEY_T, VALUE_T),
                                        ByVal that As unordered_map(Of KEY_T, VALUE_T)) As Boolean
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T), _true, first_const_pair_hasher, first_const_pair_equaler) _
                   .swap(this, that)
    End Function
End Class