
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_map.vbp ----------
'so change unordered_map.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template


Partial Public Class unordered_map( _
                         Of KEY_T,
                            VALUE_T,
                            _HASHER As _to_uint32(Of KEY_T),
                            _EQUALER As _equaler(Of KEY_T))
    Inherits hasharray( _
                 Of first_const_pair(Of KEY_T, VALUE_T),
                    _true,
                    first_const_pair.first_hasher(Of KEY_T, VALUE_T, _HASHER),
                    first_const_pair.first_equaler(Of KEY_T, VALUE_T, _EQUALER))
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T, _HASHER, _EQUALER))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hasharray.copy_constructor.vbp ----------
'so change hasharray.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of hasher_node(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

'finish hasharray.copy_constructor.vbp --------

End Class

Partial Public NotInheritable Class unordered_map(Of KEY_T, VALUE_T)
    Inherits unordered_map(Of KEY_T, VALUE_T, fast_to_uint32(Of KEY_T), default_equaler(Of KEY_T))
    Implements ICloneable, ICloneable(Of unordered_map(Of KEY_T, VALUE_T)), IEquatable(Of unordered_map(Of KEY_T, VALUE_T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hasharray.copy_constructor.vbp ----------
'so change hasharray.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of hasher_node(Of first_const_pair(Of KEY_T, VALUE_T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

'finish hasharray.copy_constructor.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\map.container_operator.vbp ----------
'so change ..\binary_tree\codegen\map.container_operator.vbp instead of this file


    Shared Sub New()
        container_operator(Of unordered_map(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).size(
                Function(ByVal i As unordered_map(Of KEY_T, VALUE_T)) As UInt32
                    assert(Not i Is Nothing)
                    Return i.size()
                End Function)
        container_operator(Of unordered_map(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).emplace(
                Function(ByVal i As unordered_map(Of KEY_T, VALUE_T),
                         ByVal j As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
                    assert(Not i Is Nothing)
                    Return i.emplace(j).second
                End Function)
        container_operator(Of unordered_map(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerate(
                Function(ByVal i As unordered_map(Of KEY_T, VALUE_T)) _
                        As container_operator(Of unordered_map(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator
                    Return New enumerator(i)
                End Function)
        container_operator(Of unordered_map(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).clear(
                Sub(ByVal i As unordered_map(Of KEY_T, VALUE_T))
                    assert(Not i Is Nothing)
                    i.clear()
                End Sub)
        bytes_serializer(Of unordered_map(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register()
        json_serializer(Of unordered_map(Of KEY_T, VALUE_T)).container(Of first_const_pair(Of KEY_T, VALUE_T)).register_as_object()
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
    Public Function second_mapper(Of VALUE2_T)(ByVal f As Func(Of VALUE_T, VALUE2_T)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE2_T))
        assert(Not f Is Nothing)
        Return Function(ByVal p As first_const_pair(Of KEY_T, VALUE_T)) As first_const_pair(Of KEY_T, VALUE2_T)
                   assert(Not p Is Nothing)
                   Return first_const_pair.of(p.first, f(p.second))
               End Function
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function first_filter(ByVal f As Func(Of KEY_T, Boolean)) As Func(Of first_const_pair(Of KEY_T, VALUE_T), Boolean)
        assert(Not f Is Nothing)
        Return Function(ByVal i As first_const_pair(Of KEY_T, VALUE_T)) As Boolean
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

    Private NotInheritable Class enumerator
        Implements container_operator(Of unordered_map(Of KEY_T, VALUE_T), first_const_pair(Of KEY_T, VALUE_T)).enumerator

        Private it As unordered_map(Of KEY_T, VALUE_T).iterator

        Public Sub New(ByVal m As unordered_map(Of KEY_T, VALUE_T))
            assert(Not m Is Nothing)
            it = m.begin()
        End Sub

        Public Sub [next]() Implements container_operator(Of unordered_map(Of KEY_T, VALUE_T),
                                                             first_const_pair(Of KEY_T, VALUE_T)).enumerator.next
            it += 1
        End Sub

        Public Function current() As first_const_pair(Of KEY_T, VALUE_T) _
                Implements container_operator(Of unordered_map(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.current
            Return +it
        End Function

        Public Function [end]() As Boolean _
                Implements container_operator(Of unordered_map(Of KEY_T, VALUE_T),
                                                 first_const_pair(Of KEY_T, VALUE_T)).enumerator.end
            Return it.is_end()
        End Function
    End Class

'finish ..\binary_tree\codegen\map.container_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\map.compare.vbp ----------
'so change ..\binary_tree\codegen\map.compare.vbp instead of this file


    ' Returns a new unordered_map containing all pairs in Me but not in that,
    ' including unequal values.
    Public Function exclude(ByVal that As unordered_map(Of KEY_T, VALUE_T)) As unordered_map(Of KEY_T, VALUE_T)
        If that Is Nothing OrElse that.empty() Then
            Return CloneT()
        End If
  
        Dim r As unordered_map(Of KEY_T, VALUE_T) = Nothing
        r = New unordered_map(Of KEY_T, VALUE_T)()
        Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
        it = begin()
        While it <> [end]()
            Dim tit As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
            tit = that.find((+it).first)
            If tit = that.end() OrElse Not equal((+it).second, (+tit).second) Then
                assert(r.insert((+it).first, (+it).second).second)
            End If
            it += 1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As unordered_map(Of KEY_T, VALUE_T)) As Boolean _
            Implements IEquatable(Of unordered_map(Of KEY_T, VALUE_T)).Equals
        If that Is Nothing OrElse that.empty() Then
            Return empty()
        End If
        Return exclude(that).empty() AndAlso that.exclude(Me).empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function Equals(ByVal that As Object) As Boolean
        Return Equals(cast(Of unordered_map(Of KEY_T, VALUE_T))(that, False))
    End Function
'finish ..\binary_tree\codegen\map.compare.vbp --------

End Class
'finish unordered_map.vbp --------
