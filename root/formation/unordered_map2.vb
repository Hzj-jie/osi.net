
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_map2.vbp ----------
'so change unordered_map2.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_map.vbp ----------
'so change unordered_map.vbp instead of this file


#Const PAIR_IS_CLASS = ("first_const_pair" = "first_const_pair")
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class unordered_map2( _
                         Of KEY_T,
                            VALUE_T,
                            _HASHER As _to_uint32(Of KEY_T),
                            _EQUALER As _equaler(Of KEY_T))
    Inherits hashtable( _
                 Of first_const_pair(Of KEY_T, VALUE_T),
                    _true,
                    first_const_pair_hasher,
                    first_const_pair_equaler)
    Implements ICloneable, ICloneable(Of unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hashtable.copy_constructor.vbp ----------
'so change hashtable.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub
'finish hashtable.copy_constructor.vbp --------

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER))() As R
        Return MyBase.clone(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER) _
            Implements ICloneable(Of unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER)).Clone
        Return clone(Of unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) _
                                       As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER)
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair_hasher,
                            first_const_pair_equaler) _
                   .move(Of unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER),
                                        ByVal that As unordered_map2(Of KEY_T, VALUE_T, _HASHER, _EQUALER)) As Boolean
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T),
                            _true,
                            first_const_pair_hasher,
                            first_const_pair_equaler) _
                   .swap(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function emplace(ByVal key As KEY_T, ByVal value As VALUE_T) As fast_pair(Of iterator, Boolean)
        Return MyBase.emplace(first_const_pair.emplace_of(key, value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function insert(ByVal key As KEY_T, ByVal value As VALUE_T) As fast_pair(Of iterator, Boolean)
        Return emplace(copy_no_error(key), copy_no_error(value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        Return MyBase.erase(it)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal key As KEY_T) As Boolean
        Return MyBase.erase(find(key))
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
            Dim r As fast_pair(Of iterator, Boolean) = Nothing
            r = insert(key, value)
            If Not r.second Then
                copy(r.first.value().second, value)
            End If
        End Set
    End Property

    Public NotInheritable Class first_const_pair_hasher
        Inherits _to_uint32(Of first_const_pair(Of KEY_T, VALUE_T))

        Private Shared ReadOnly hasher As _HASHER

        Shared Sub New()
            hasher = alloc(Of _HASHER)()
        End Sub

        Public Overrides Function at(ByRef k As first_const_pair(Of KEY_T, VALUE_T)) As UInt32
#If PAIR_IS_CLASS Then
            assert(Not k Is Nothing)
#End If
            Return hasher(k.first)
        End Function

        Public Overrides Function reverse(ByVal i As UInt32) As first_const_pair(Of KEY_T, VALUE_T)
            assert(False)
            Return Nothing
        End Function
    End Class

    Public NotInheritable Class first_const_pair_equaler
        Inherits _equaler(Of first_const_pair(Of KEY_T, VALUE_T))

        Private Shared ReadOnly equaler As _EQUALER

        Shared Sub New()
            equaler = alloc(Of _EQUALER)()
        End Sub

        Public Overrides Function at(ByRef i As first_const_pair(Of KEY_T, VALUE_T),
                                     ByRef j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
#If PAIR_IS_CLASS Then
            Dim c As Int32 = 0
            c = object_compare(i, j)
            If c <> object_compare_undetermined Then
                Return c = 0
            End If
            assert(Not i Is Nothing)
            assert(Not j Is Nothing)
#End If
            Return equaler(i.first, j.first)
        End Function
    End Class
End Class

Public Class unordered_map2(Of KEY_T, VALUE_T)
    Inherits unordered_map2(Of KEY_T, VALUE_T, fast_to_uint32(Of KEY_T), default_equaler(Of KEY_T))
    Implements ICloneable, ICloneable(Of unordered_map2(Of KEY_T, VALUE_T)), IEquatable(Of unordered_map2(Of KEY_T, VALUE_T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hashtable.copy_constructor.vbp ----------
'so change hashtable.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub
'finish hashtable.copy_constructor.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\map.container_operator.vbp ----------
'so change ..\binary_tree\codegen\map.container_operator.vbp instead of this file


    Shared Sub New()
        container_operator(Of unordered_map2(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).size(
                Function(ByVal i As unordered_map2(Of KEY_T, VALUE_T)) As UInt32
                    assert(Not i Is Nothing)
                    Return i.size()
                End Function)
        container_operator(Of unordered_map2(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).emplace(
                Function(ByVal i As unordered_map2(Of KEY_T, VALUE_T),
                         ByVal j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                    assert(Not i Is Nothing)
                    Return i.emplace(j).second
                End Function)
        container_operator(Of unordered_map2(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerate(
                Function(ByVal i As unordered_map2(Of KEY_T, VALUE_T)) _
                        As container_operator(Of unordered_map2(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator
                    Return New enumerator(i)
                End Function)
        container_operator(Of unordered_map2(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).clear(
                Sub(ByVal i As unordered_map2(Of KEY_T, VALUE_T))
                    assert(Not i Is Nothing)
                    i.clear()
                End Sub)
        bytes_serializer(Of unordered_map2(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register()
        json_serializer(Of unordered_map2(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register_as_object()
    End Sub

    Public ReadOnly first_selector As Func(Of first_const_pair(Of KEY_T, VALUE_T), KEY_T) =
        first_const_pair(Of KEY_T, VALUE_T).first_getter

    Public ReadOnly second_selector As Func(Of first_const_pair(Of KEY_T, VALUE_T), VALUE_T) =
        first_const_pair(Of KEY_T, VALUE_T).second_getter

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function first_mapper(Of KEY2_T)(ByVal f As Func(Of KEY_T, KEY2_T)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), first_const_pair(Of KEY2_T, VALUE_T))
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As first_const_pair(Of KEY2_T, VALUE_T)
                   assert(Not p Is Nothing)
                   Return first_const_pair.of(f(p.first), p.second)
               End Function
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function second_mapper(Of VALUE2_T)(ByVal f As Func(Of VALUE_T, VALUE2_T)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE2_T))
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As first_const_pair(Of KEY_T, VALUE2_T)
                   assert(Not p Is Nothing)
                   Return first_const_pair.of(p.first, f(p.second))
               End Function
    End Function

    Private NotInheritable Class enumerator
        Implements container_operator(Of unordered_map2(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerator

        Private it As unordered_map2(Of KEY_T, VALUE_T).iterator

        Public Sub New(ByVal m As unordered_map2(Of KEY_T, VALUE_T))
            assert(Not m Is Nothing)
            it = m.begin()
        End Sub

        Public Sub [next]() Implements container_operator(Of unordered_map2(Of KEY_T, VALUE_T),
                                                             first_const_pair(Of KEY_T, VALUE_T)).enumerator.next
            it += 1
        End Sub

        Public Function current() As first_const_pair(Of KEY_T, VALUE_T) _
                Implements container_operator(Of unordered_map2(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.current
            Return +it
        End Function

        Public Function [end]() As Boolean _
                Implements container_operator(Of unordered_map2(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.end
            Return it.is_end()
        End Function
    End Class

'finish ..\binary_tree\codegen\map.container_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\map.compare.vbp ----------
'so change ..\binary_tree\codegen\map.compare.vbp instead of this file


    ' Returns a new unordered_map2 containing all pairs in Me but not in that,
    ' including unequal values.
    Public Function exclude(ByVal that As unordered_map2(Of KEY_T, VALUE_T)) As unordered_map2(Of KEY_T, VALUE_T)
        If that Is Nothing OrElse that.empty() Then
            Return CloneT()
        End If
  
        Dim r As unordered_map2(Of KEY_T, VALUE_T) = Nothing
        r = New unordered_map2(Of KEY_T, VALUE_T)()
        Dim it As unordered_map2(Of KEY_T, VALUE_T).iterator = Nothing
        it = begin()
        While it <> [end]()
            Dim tit As unordered_map2(Of KEY_T, VALUE_T).iterator = Nothing
            tit = that.find((+it).first)
            If tit = that.end() OrElse Not equal((+it).second, (+tit).second) Then
                assert(r.insert((+it).first, (+it).second).second)
            End If
            it += 1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As unordered_map2(Of KEY_T, VALUE_T)) As Boolean _
            Implements IEquatable(Of unordered_map2(Of KEY_T, VALUE_T)).Equals
        If that Is Nothing OrElse that.empty() Then
            Return empty()
        End If
        Return exclude(that).empty() AndAlso that.exclude(Me).empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function Equals(ByVal that As Object) As Boolean
        Return Equals(cast(Of unordered_map2(Of KEY_T, VALUE_T))(that, False))
    End Function
'finish ..\binary_tree\codegen\map.compare.vbp --------

    Public Sub New()
        MyBase.New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shadows Function clone(Of R As unordered_map2(Of KEY_T, VALUE_T))() As R
        Return MyBase.clone(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_map2(Of KEY_T, VALUE_T) _
            Implements ICloneable(Of unordered_map2(Of KEY_T, VALUE_T)).Clone
        Return MyBase.clone(Of unordered_map2(Of KEY_T, VALUE_T))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_map2(Of KEY_T, VALUE_T)) _
                                       As unordered_map2(Of KEY_T, VALUE_T)
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T),
                                _true,
                                first_const_pair_hasher,
                                first_const_pair_equaler) _
                   .move(Of unordered_map2(Of KEY_T, VALUE_T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_map2(Of KEY_T, VALUE_T),
                                        ByVal that As unordered_map2(Of KEY_T, VALUE_T)) As Boolean
        Return hashtable(Of first_const_pair(Of KEY_T, VALUE_T),
                                _true,
                                first_const_pair_hasher,
                                first_const_pair_equaler) _
                   .swap(this, that)
    End Function
End Class
'finish unordered_map.vbp --------
'finish unordered_map2.vbp --------
