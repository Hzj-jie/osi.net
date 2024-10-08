
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with smap.vbp ----------
'so change smap.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.impl.vbp ----------
'so change map.impl.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class smap(Of KEY_T, VALUE_T)
    Inherits sbst(Of first_const_pair(Of KEY_T, VALUE_T))
    Implements ICloneable, ICloneable(Of smap(Of KEY_T, VALUE_T)), IEquatable(Of smap(Of KEY_T, VALUE_T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.container_operator.vbp ----------
'so change map.container_operator.vbp instead of this file


    Shared Sub New()
        container_operator(Of smap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).size(
                Function(ByVal i As smap(Of KEY_T, VALUE_T)) As UInt32
                    assert(Not i Is Nothing)
                    Return i.size()
                End Function)
        container_operator(Of smap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).emplace(
                Function(ByVal i As smap(Of KEY_T, VALUE_T),
                         ByVal j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                    assert(Not i Is Nothing)
                    Return i.emplace(j).second
                End Function)
        container_operator(Of smap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerate(
                Function(ByVal i As smap(Of KEY_T, VALUE_T)) _
                        As container_operator(Of first_const_pair(Of KEY_T, VALUE_T)).enumerator
                    assert(Not i Is Nothing)
                    Return New enumerator(i)
                End Function)
        container_operator(Of smap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).clear(
                Sub(ByVal i As smap(Of KEY_T, VALUE_T))
                    assert(Not i Is Nothing)
                    i.clear()
                End Sub)
        bytes_serializer(Of smap(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register()
        json_serializer(Of smap(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register_as_object()
        string_serializer(Of smap(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function on_first(ByVal f As Action(Of KEY_T)) As Action(Of first_const_pair(Of KEY_T, VALUE_T))
        assert(Not f Is Nothing)
        Return Sub(ByVal i As first_const_pair(Of KEY_T, VALUE_T))
                   assert(Not i Is Nothing)
                   f(i.first)
               End Sub
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function on_second(ByVal f As Action(Of VALUE_T)) As Action(Of first_const_pair(Of KEY_T, VALUE_T))
        assert(Not f Is Nothing)
        Return Sub(ByVal i As first_const_pair(Of KEY_T, VALUE_T))
                   assert(Not i Is Nothing)
                   f(i.second)
               End Sub
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function on_pair(ByVal f As Action(Of KEY_T, VALUE_T)) As Action(Of first_const_pair(Of KEY_T, VALUE_T))
        assert(Not f Is Nothing)
        Return Sub(ByVal i As first_const_pair(Of KEY_T, VALUE_T))
                   assert(Not i Is Nothing)
                   f(i.first, i.second)
               End Sub
    End Function

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
    Public Function first_mapper(Of KEY2_T)(ByVal f As Func(Of KEY_T, VALUE_T, KEY2_T)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), first_const_pair(Of KEY2_T, VALUE_T))
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As first_const_pair(Of KEY2_T, VALUE_T)
                   assert(Not p Is Nothing)
                   Return first_const_pair.of(f(p.first, p.second), p.second)
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function second_mapper(Of VALUE2_T)(ByVal f As Func(Of KEY_T, VALUE_T, VALUE2_T)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE2_T))
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As first_const_pair(Of KEY_T, VALUE2_T)
                   assert(Not p Is Nothing)
                   Return first_const_pair.of(p.first, f(p.first, p.second))
               End Function
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function first_filter(ByVal f As Func(Of KEY_T, Boolean)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), Boolean)
        assert(Not f Is Nothing)
        Return Function(ByVal i As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                   assert(Not i Is Nothing)
                   Return f(i.first)
               End Function
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function second_filter(ByVal f As Func(Of VALUE_T, Boolean)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), Boolean)
        assert(Not f Is Nothing)
        Return Function(ByVal i As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                   Return f(i.second)
               End Function
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function mapper(Of R)(ByVal f As Func(Of KEY_T, VALUE_T, R)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), R)
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As R
                   assert(Not p Is Nothing)
                   Return f(p.first, p.second)
               End Function
    End Function
'finish map.container_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.compare.vbp ----------
'so change map.compare.vbp instead of this file


    ' Returns a new smap containing all pairs in Me but not in that,
    ' including unequal values.
    Public Function exclude(ByVal that As smap(Of KEY_T, VALUE_T)) As smap(Of KEY_T, VALUE_T)
        If that Is Nothing OrElse that.empty() Then
            Return CloneT()
        End If
  
        Dim r As smap(Of KEY_T, VALUE_T) = Nothing
        r = New smap(Of KEY_T, VALUE_T)()
        Dim it As smap(Of KEY_T, VALUE_T).iterator = Nothing
        it = begin()
        While it <> [end]()
            Dim tit As smap(Of KEY_T, VALUE_T).iterator = Nothing
            tit = that.find((+it).first)
            If tit = that.end() OrElse Not equal((+it).second, (+tit).second) Then
                assert(r.insert((+it).first, (+it).second).second)
            End If
            it += 1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As smap(Of KEY_T, VALUE_T)) As Boolean _
            Implements IEquatable(Of smap(Of KEY_T, VALUE_T)).Equals
        If that Is Nothing OrElse that.empty() Then
            Return empty()
        End If
        Return exclude(that).empty() AndAlso that.exclude(Me).empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function Equals(ByVal that As Object) As Boolean
        Return Equals(cast(Of smap(Of KEY_T, VALUE_T))(that, False))
    End Function
'finish map.compare.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.operators.vbp ----------
'so change map.operators.vbp instead of this file


    Private Function emplace_merge_copied_pair(ByVal v As first_const_pair(Of KEY_T, VALUE_T),
                                               ByVal merger as Func(Of VALUE_T, VALUE_T, VALUE_T)) As VALUE_T
        assert(Not v Is Nothing)
        assert(Not merger Is Nothing)
        Dim it As iterator = find(v.first)
        If it = [end]() Then
            assert(emplace(v).second)
            Return v.second
        End If
        Dim n As VALUE_T = merger((+it).second, v.second)
        assert([erase](v.first))
        assert(emplace(v.first, n).second)
        Return n
    End Function

    Public Function emplace_merge(ByVal k As KEY_T,
                                  ByVal v As VALUE_T,
                                  ByVal merger As Func(Of VALUE_T, VALUE_T, VALUE_T)) As VALUE_T
        Return emplace_merge_copied_pair(first_const_pair.emplace_of(k, v), merger)
    End Function

    Public Function emplace_merge(ByVal v As first_const_pair(Of KEY_T, VALUE_T),
                                  ByVal merger As Func(Of VALUE_T, VALUE_T, VALUE_T)) As VALUE_T
        Return emplace_merge_copied_pair(v, merger)
    End Function

    Public Function merge(ByVal k As KEY_T,
                          ByVal v As VALUE_T,
                          ByVal merger As Func(Of VALUE_T, VALUE_T, VALUE_T)) As VALUE_T
        Return emplace_merge_copied_pair(first_const_pair.of(k, v), merger)
    End Function

    Public Function merge(ByVal v As first_const_pair(Of KEY_T, VALUE_T),
                          ByVal merger As Func(Of VALUE_T, VALUE_T, VALUE_T)) As VALUE_T
        Return emplace_merge_copied_pair(copy_no_error(v), merger)
    End Function

    Public Shared Operator +(ByVal this As smap(Of KEY_T, VALUE_T),
                             ByVal v As first_const_pair(Of KEY_T, VALUE_T)) as smap(Of KEY_T, VALUE_T)

        assert(Not this Is Nothing)
        assert(this.insert(v).second)
        Return this
    End Operator
'finish map.operators.vbp --------

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        MyBase.New(first_const_pair(Of KEY_T, VALUE_T).first_comparer)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As smap(Of KEY_T, VALUE_T)) As smap(Of KEY_T, VALUE_T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As smap(Of KEY_T, VALUE_T) = Nothing
        r = New smap(Of KEY_T, VALUE_T)()
        move_to(v, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function clone() As smap(Of KEY_T, VALUE_T)
        Dim r As smap(Of KEY_T, VALUE_T) = Nothing
        r = New smap(Of KEY_T, VALUE_T)()
        clone_to(Me, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As smap(Of KEY_T, VALUE_T) Implements ICloneable(Of smap(Of KEY_T, VALUE_T)).Clone
        Return clone()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function emplace(ByVal key As KEY_T,
                                      ByVal value As VALUE_T) As tuple(Of iterator, Boolean)
        Return MyBase.emplace(first_const_pair.emplace_of(key, value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function insert(ByVal key As KEY_T,
                                     ByVal value As VALUE_T) As tuple(Of iterator, Boolean)
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
        [erase](it)
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
'finish map.impl.vbp --------
'finish smap.vbp --------
