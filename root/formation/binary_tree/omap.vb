
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with omap.vbp ----------
'so change omap.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.impl.vbp ----------
'so change map.impl.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class omap(Of KEY_T, VALUE_T)
    Inherits obst(Of first_const_pair(Of KEY_T, VALUE_T))
    Implements ICloneable, ICloneable(Of omap(Of KEY_T, VALUE_T)), IEquatable(Of omap(Of KEY_T, VALUE_T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.container_operator.vbp ----------
'so change map.container_operator.vbp instead of this file


    Shared Sub New()
        container_operator(Of omap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).size(
                Function(ByVal i As omap(Of KEY_T, VALUE_T)) As UInt32
                    assert(Not i Is Nothing)
                    Return i.size()
                End Function)
        container_operator(Of omap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).emplace(
                Function(ByVal i As omap(Of KEY_T, VALUE_T),
                         ByVal j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                    assert(Not i Is Nothing)
                    Return i.emplace(j).second
                End Function)
        container_operator(Of omap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerate(
                Function(ByVal i As omap(Of KEY_T, VALUE_T)) _
                        As container_operator(Of omap(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator
                    Return New enumerator(i)
                End Function)
        container_operator(Of omap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).clear(
                Sub(ByVal i As omap(Of KEY_T, VALUE_T))
                    assert(Not i Is Nothing)
                    i.clear()
                End Sub)
        bytes_serializer(Of omap(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register()
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
        Implements container_operator(Of omap(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerator

        Private it As omap(Of KEY_T, VALUE_T).iterator

        Public Sub New(ByVal m As omap(Of KEY_T, VALUE_T))
            assert(Not m Is Nothing)
            it = m.begin()
        End Sub

        Public Sub [next]() Implements container_operator(Of omap(Of KEY_T, VALUE_T),
                                                             first_const_pair(Of KEY_T, VALUE_T)).enumerator.next
            it += 1
        End Sub

        Public Function current() As first_const_pair(Of KEY_T, VALUE_T) _
                Implements container_operator(Of omap(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.current
            Return +it
        End Function

        Public Function [end]() As Boolean _
                Implements container_operator(Of omap(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.end
            Return it.is_end()
        End Function
    End Class

'finish map.container_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.compare.vbp ----------
'so change map.compare.vbp instead of this file


    ' Returns a new omap containing all pairs in Me but not in that,
    ' including unequal values.
    Public Function exclude(ByVal that As omap(Of KEY_T, VALUE_T)) As omap(Of KEY_T, VALUE_T)
        If that Is Nothing OrElse that.empty() Then
            Return CloneT()
        End If
  
        Dim r As omap(Of KEY_T, VALUE_T) = Nothing
        r = New omap(Of KEY_T, VALUE_T)()
        Dim it As omap(Of KEY_T, VALUE_T).iterator = Nothing
        it = begin()
        While it <> [end]()
            Dim tit As omap(Of KEY_T, VALUE_T).iterator = Nothing
            tit = that.find((+it).first)
            If tit = that.end() OrElse Not equal((+it).second, (+tit).second) Then
                assert(r.insert((+it).first, (+it).second).second)
            End If
            it += 1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As omap(Of KEY_T, VALUE_T)) As Boolean _
            Implements IEquatable(Of omap(Of KEY_T, VALUE_T)).Equals
        If that Is Nothing OrElse that.empty() Then
            Return empty()
        End If
        Return exclude(that).empty() AndAlso that.exclude(Me).empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function Equals(ByVal that As Object) As Boolean
        Return Equals(cast(Of omap(Of KEY_T, VALUE_T))(that, False))
    End Function
'finish map.compare.vbp --------

    Public Sub New()
        MyBase.New(AddressOf first_compare)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function first_compare(ByVal this As first_const_pair(Of KEY_T, VALUE_T),
                                          ByVal that As first_const_pair(Of KEY_T, VALUE_T)) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return connector.compare(this.first, that.first)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As omap(Of KEY_T, VALUE_T)) As omap(Of KEY_T, VALUE_T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As omap(Of KEY_T, VALUE_T) = Nothing
        r = New omap(Of KEY_T, VALUE_T)()
        move_to(v, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function clone() As omap(Of KEY_T, VALUE_T)
        Dim r As omap(Of KEY_T, VALUE_T) = Nothing
        r = New omap(Of KEY_T, VALUE_T)()
        clone_to(Me, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As omap(Of KEY_T, VALUE_T) Implements ICloneable(Of omap(Of KEY_T, VALUE_T)).Clone
        Return clone()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function emplace(ByVal key As KEY_T,
                                      ByVal value As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.emplace(first_const_pair.emplace_of(key, value))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function insert(ByVal key As KEY_T,
                                     ByVal value As VALUE_T) As pair(Of iterator, Boolean)
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
            Dim r As pair(Of iterator, Boolean) = Nothing
            r = insert(key, value)
            If Not r.second Then
                copy(r.first.value().second, value)
            End If
        End Set
    End Property
End Class
'finish map.impl.vbp --------
'finish omap.vbp --------
