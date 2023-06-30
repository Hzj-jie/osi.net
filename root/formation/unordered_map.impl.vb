
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class unordered_map( _
                         Of KEY_T,
                            VALUE_T,
                            _HASHER As _to_uint32(Of KEY_T),
                            _EQUALER As _equaler(Of KEY_T))
    Inherits hasharray(
                 Of first_const_pair(Of KEY_T, VALUE_T),
                    _true,
                    first_const_pair(Of KEY_T, VALUE_T).first_hasher(Of _HASHER),
                    first_const_pair(Of KEY_T, VALUE_T).first_equaler(Of _EQUALER))
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))() As R
        Return MyBase.clone(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER) _
            Implements ICloneable(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)).Clone
        Return clone(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) _
                                       As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)
        Return hasharray(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair(Of KEY_T, VALUE_T).first_hasher(Of _HASHER),
                            first_const_pair(Of KEY_T, VALUE_T).first_equaler(Of _EQUALER)) _
                   .move(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER),
                                        ByVal that As unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) As Boolean
        Return hasharray(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair(Of KEY_T, VALUE_T).first_hasher(Of _HASHER),
                            first_const_pair(Of KEY_T, VALUE_T).first_equaler(Of _EQUALER)) _
                   .swap(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function emplace(ByVal key As KEY_T, ByVal value As VALUE_T) As tuple(Of iterator, Boolean)
        Return MyBase.emplace(first_const_pair.emplace_of(key, value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function insert(ByVal key As KEY_T, ByVal value As VALUE_T) As tuple(Of iterator, Boolean)
        Return emplace(copy_no_error(key), copy_no_error(value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal it As iterator) As iterator
        Return MyBase.erase(it)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal key As KEY_T) As Boolean
        Dim it As iterator = find(key)
        If it = [end]() Then
            Return False
        End If
        MyBase.[erase](it)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function find(ByVal key As KEY_T) As iterator
        Return MyBase.find(first_const_pair(Of KEY_T, VALUE_T).emplace_of(key))
    End Function

    Default Public Property at(ByVal key As KEY_T) As VALUE_T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Dim r As iterator = Nothing
            r = find(key)
            If r = [end]() Then
                r = emplace(copy_no_error(key), alloc(Of VALUE_T)()).first
            End If
            Return (+r).second
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Set(ByVal value As VALUE_T)
            Dim r As tuple(Of iterator, Boolean) = Nothing
            r = insert(key, value)
            If Not r.second Then
                copy(r.first.value().second, value)
            End If
        End Set
    End Property
End Class

Partial Public NotInheritable Class unordered_map(Of KEY_T, VALUE_T)
    Inherits unordered_map(Of KEY_T, VALUE_T, fast_to_uint32(Of KEY_T), default_equaler(Of KEY_T))
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T)), IEquatable(Of unordered_map(Of KEY_T, VALUE_T))

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        MyBase.New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_map(Of KEY_T, VALUE_T) _
            Implements ICloneable(Of unordered_map(Of KEY_T, VALUE_T)).Clone
        Return MyBase.clone(Of unordered_map(Of KEY_T, VALUE_T))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map(Of KEY_T, VALUE_T)) _
                                       As unordered_map(Of KEY_T, VALUE_T)
        Return hasharray(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair(Of KEY_T, VALUE_T).first_hasher(Of fast_to_uint32(Of KEY_T)),
                            first_const_pair(Of KEY_T, VALUE_T).first_equaler(Of default_equaler(Of KEY_T))) _
                   .move(Of unordered_map(Of KEY_T, VALUE_T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map(Of KEY_T, VALUE_T),
                                        ByVal that As unordered_map(Of KEY_T, VALUE_T)) As Boolean
        Return hasharray(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair(Of KEY_T, VALUE_T).first_hasher(Of fast_to_uint32(Of KEY_T)),
                            first_const_pair(Of KEY_T, VALUE_T).first_equaler(Of default_equaler(Of KEY_T))) _
                   .swap(this, that)
    End Function
End Class
